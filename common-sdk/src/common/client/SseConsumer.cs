using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;
using EvtSource;
using NLog;
using Timer = System.Timers.Timer;
using SseHandler = System.Func<EvtSource.EventSourceMessageEventArgs, bool>;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;

namespace Coin.Sdk.Common.Client
{
    public class SseConsumer : CtpApiRestTemplateSupport
    {
        private const long DefaultOffset = -1;
        private const int DefaultNumberOfRetries = 15;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Uri _sseUri;
        private EventSourceReader _eventSourceReader;
        private readonly ReadTimeoutTimer _timer = new ReadTimeoutTimer();
        private readonly BackoffHandler _backoffHandler;

        private enum ConfirmationStatus
        {
            Unconfirmed,
            All
        }

        public SseConsumer(string consumerName, string sseUri, string privateKeyFile, string encryptedHmacSecretFile,
            int backOffPeriod = 1, int numberOfRetries = DefaultNumberOfRetries) :
            this(consumerName, new Uri(sseUri), ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile,
                backOffPeriod, numberOfRetries)
        {
        }

        public SseConsumer(string consumerName, Uri sseUri, string privateKeyFile, string encryptedHmacSecretFile,
            int backOffPeriod = 1, int numberOfRetries = DefaultNumberOfRetries) :
            this(consumerName, sseUri, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile, backOffPeriod,
                numberOfRetries)
        {
        }

        private SseConsumer(string consumerName, Uri sseUri, RSA privateKey, string encryptedHmacSecretFile, int backOffPeriod,
            int numberOfRetries) :
            this(consumerName, sseUri, privateKey,
                HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey), backOffPeriod,
                numberOfRetries)
        {
        }

        public SseConsumer(string consumerName, string sseUri, RSA privateKey, HMACSHA256 signer, int backOffPeriod = 1,
            int numberOfRetries = DefaultNumberOfRetries) : this(consumerName, new Uri(sseUri), privateKey, signer,
            backOffPeriod)
        {
        }

        public SseConsumer(string consumerName, Uri sseUri, RSA privateKey, HMACSHA256 signer, int backOffPeriod = 1,
            int numberOfRetries = DefaultNumberOfRetries) : base(consumerName, privateKey, signer)
        {
            _sseUri = sseUri;
            _backoffHandler = new BackoffHandler(backOffPeriod, numberOfRetries);
        }

        public void StopConsuming()
        {
            Debug.WriteLine("Stopped consuming messages");
            _timer.Stop();
            if (_eventSourceReader?.IsDisposed == false) _eventSourceReader.Dispose();
        }

        public void StartConsumingUnconfirmed(
            SseHandler handleSse,
            IEnumerable<string> messageTypes = null,
            Dictionary<string, IEnumerable<string>> otherParams = null,
            Action<Exception> onFinalDisconnect = null) =>
            StartConsuming(handleSse, ConfirmationStatus.Unconfirmed, messageTypes, otherParams, onFinalDisconnect);

        public void StartConsumingUnconfirmedWithOffsetPersistence(
            SseHandler handleSse,
            IOffsetPersister offsetPersister,
            long offset = DefaultOffset,
            IEnumerable<string> messageTypes = null,
            Dictionary<string, IEnumerable<string>> otherParams = null,
            Action<Exception> onFinalDisconnect = null) =>
            StartConsuming(handleSse, ConfirmationStatus.Unconfirmed, messageTypes, otherParams, onFinalDisconnect,
                offsetPersister, offset);

        public void StartConsumingAll(
            SseHandler handleSse,
            IOffsetPersister offsetPersister,
            long offset = DefaultOffset,
            IEnumerable<string> messageTypes = null,
            Dictionary<string, IEnumerable<string>> otherParams = null,
            Action<Exception> onFinalDisconnect = null) =>
            StartConsuming(handleSse, ConfirmationStatus.All, messageTypes, otherParams, onFinalDisconnect,
                offsetPersister, offset);

        private void StartConsuming(SseHandler handleSse,
            ConfirmationStatus confirmationStatus = ConfirmationStatus.Unconfirmed,
            IEnumerable<string> messageTypes = null,
            Dictionary<string, IEnumerable<string>> otherParams = null,
            Action<Exception> onFinalDisconnect = null,
            IOffsetPersister offsetPersister = null,
            long offset = DefaultOffset)
        {
            _timer.SetToken(CoinHttpClientHandler.CancellationTokenSource);
            _eventSourceReader = new EventSourceReader(CreateUri(offset, confirmationStatus, messageTypes, otherParams),
                CoinHttpClientHandler);

            StartReading();

            void StartReading()
            {
                _eventSourceReader.MessageReceived += (sender, e) => HandleEvent(e);
                _eventSourceReader.Start();
                _eventSourceReader.Disconnected += (sender, e) => HandleDisconnect(e);
                _logger.Info("Stream started");
                _timer.Start();
            }

            void HandleEvent(EventSourceMessageEventArgs messageEvent)
            {
                _timer.UpdateTimestamp();

                var success = handleSse(messageEvent);
                if (success && offsetPersister != null)
                    offsetPersister.Offset = long.Parse(messageEvent.Id, CultureInfo.InvariantCulture);

                _backoffHandler.Reset();
                _timer.Reset();
            }

            void HandleDisconnect(DisconnectEventArgs e)
            {
                _timer.Stop();
                _logger.Debug($"Error: {e.Exception.Message}");
                var persistedOffset = offsetPersister?.Offset ?? offset;
                if (_backoffHandler.MaximumNumberOfRetriesUsed())
                {
                    _logger.Error("Reached maximum number of connection retries, stopped consuming event stream.");
                    onFinalDisconnect?.Invoke(e.Exception);
                    return;
                }

                _backoffHandler.WaitBackOffPeriod();

                _logger.Debug("Restarting stream");
                _eventSourceReader = new EventSourceReader(
                    CreateUri(persistedOffset, confirmationStatus, messageTypes, otherParams), CoinHttpClientHandler);
                CoinHttpClientHandler.CancellationTokenSource = new CancellationTokenSource();
                _timer.SetToken(CoinHttpClientHandler.CancellationTokenSource);
                StartReading();
            }
        }

        private Uri CreateUri(long offset, ConfirmationStatus confirmationStatus, IEnumerable<string> messageTypes = null,
            Dictionary<string, IEnumerable<string>> otherParams = null) => _sseUri
            .AddQueryArg(nameof(offset), $"{offset}")
            .AddQueryArg(nameof(confirmationStatus), $"{confirmationStatus}")
            .AddQueryArg(nameof(messageTypes), messageTypes ?? new string[] { })
            .AddQueryArgs(otherParams ?? new Dictionary<string, IEnumerable<string>>());

        #region IDisposable Support

        protected override IEnumerable<IDisposable> DisposableFields =>
            base.DisposableFields.Concat(new IDisposable[] {_eventSourceReader, _timer });

        #endregion
    }

    internal sealed class ReadTimeoutTimer : IDisposable
    {
        private const int ThresholdTimeout = 300_000_000;
        private readonly Timer _timer = new Timer();
        private long _timestamp = DateTime.Now.Ticks;
        private CancellationTokenSource _cancellationTokenSource;

        public ReadTimeoutTimer()
        {
            _timer.Interval = 30000;
            _timer.Elapsed += OnTimedEvent;
        }

        public void UpdateTimestamp() => _timestamp = DateTime.Now.Ticks;

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public void Reset()
        {
            _timer.Stop();
            _timestamp = DateTime.Now.Ticks;
            _timer.Start();
        }

        public void SetToken(CancellationTokenSource cts) => _cancellationTokenSource = cts;

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var now = DateTime.Now.Ticks;
            var elapsedTime = now - _timestamp;
            Debug.WriteLine("Timestamp: " + _timestamp);
            Debug.WriteLine("Elapsed Time: " + elapsedTime);

            if (elapsedTime > ThresholdTimeout)
            {
                Debug.WriteLine("Timestamp: " + _timestamp);
                Debug.WriteLine("Time-out above threshold! Quitting: " + e);
                _cancellationTokenSource.Cancel();
            }
        }

        #region IDisposable Support

        private bool _disposed; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    internal class BackoffHandler
    {
        private readonly int _backoffPeriod;
        private readonly int _numberOfRetries;
        private int _currentBackoffPeriod;
        private int _retriesLeft;

        public BackoffHandler(int backOffPeriod, int numberOfRetries)
        {
            _backoffPeriod = backOffPeriod;
            _numberOfRetries = numberOfRetries;
            Reset();
        }

        public int GetBackoffPeriod() => _backoffPeriod;

        public int GetRetriesLeft() => _numberOfRetries;

        public void Reset()
        {
            _currentBackoffPeriod = _backoffPeriod;
            _retriesLeft = _numberOfRetries;
        }

        private void DecreaseNumberOfRetries()
        {
            if (_retriesLeft > 0)
                _retriesLeft--;
        }

        private void IncreaseBackOffPeriod() => _currentBackoffPeriod =
            _currentBackoffPeriod > 60 ? _currentBackoffPeriod : _currentBackoffPeriod * 2;

        public bool MaximumNumberOfRetriesUsed() => _retriesLeft <= 0;

        public void WaitBackOffPeriod()
        {
            Debug.WriteLine(
                $"Going to sleep for {_currentBackoffPeriod} seconds and still {_retriesLeft} retries left!");
            Thread.Sleep(_currentBackoffPeriod * 1000);
            IncreaseBackOffPeriod();
            DecreaseNumberOfRetries();
        }
    }

    public interface IOffsetPersister
    {
        long Offset { get; set; }
    }
}