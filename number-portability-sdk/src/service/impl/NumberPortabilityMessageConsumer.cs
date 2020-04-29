using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.src.common;
using EvtSource;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;

namespace Coin.Sdk.NP.Service.Impl
{
    public class NumberPortabilityMessageConsumer : CtpApiRestTemplateSupport
    {
        private const long _defaultoffset = -1;
        private readonly INumberPortabilityMessageListener _listener;
        private readonly Uri _sseUri;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private EventSourceReader _eventSourceReader;
        private readonly ReadTimeOutTimer _timer = new ReadTimeOutTimer();
        private readonly BackoffHandler _backoffHandler;

        public NumberPortabilityMessageConsumer(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            INumberPortabilityMessageListener listener, string sseUri, int backOffPeriod = 1, int numberOfRetries = 3,
            HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile,
                listener, new Uri(sseUri), backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds)
        { }

        public NumberPortabilityMessageConsumer(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            INumberPortabilityMessageListener listener, Uri sseUri, int backOffPeriod = 1, int numberOfRetries = 3,
            HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile,
                listener, sseUri, backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds)
        { }

        private NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, string encryptedHmacSecretFile, INumberPortabilityMessageListener listener,
            Uri sseUri, int backOffPeriod, int numberOfRetries, HmacSignatureType hmacSignatureType, int validPeriodInSeconds) :
            this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey),
                listener, sseUri, backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds)
        { }

        public NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, HMACSHA256 signer, INumberPortabilityMessageListener listener,
            string sseUri, int backOffPeriod = 1, int numberOfRetries = 3, HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest,
            int validPeriodInSeconds = DefaultValidPeriodInSecs)
            : this(consumerName, privateKey, signer, listener,
                  new Uri(sseUri), backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds)
        { }

        public NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, HMACSHA256 signer, INumberPortabilityMessageListener listener,
            Uri sseUri, int backOffPeriod = 1, int numberOfRetries = 3, HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest,
            int validPeriodInSeconds = DefaultValidPeriodInSecs) : base(consumerName, privateKey, signer, hmacSignatureType, validPeriodInSeconds)
        {
            _listener = listener;
            _sseUri = sseUri;
            _backoffHandler = new BackoffHandler(backOffPeriod, numberOfRetries);
        }

        public void StopConsuming()
        {
            Debug.WriteLine("Quitting because testcase ended!");
            _timer.Stop();
            if (_eventSourceReader?.IsDisposed == false) _eventSourceReader.Dispose();
        }

        public void StartConsuming(
            ConfirmationStatus confirmationStatus = ConfirmationStatus.Unconfirmed,
            long initialOffset = _defaultoffset,
            IOffsetPersister offsetPersister = null,
            Func<long, long> recoverOffset = null,
            Action<Exception> onFinalDisconnect = null,
            params string[] messageTypes)
        {
            if (confirmationStatus == ConfirmationStatus.All && offsetPersister == null)
                throw new InvalidEnumArgumentException("offsetPersister should be given when confirmationStatus equals All");

            CoinHttpClientHandler.CancellationTokenSource = new CancellationTokenSource();
            _timer.SetToken(CoinHttpClientHandler.CancellationTokenSource);
            _eventSourceReader = new EventSourceReader(CreateUri(initialOffset, confirmationStatus, messageTypes), CoinHttpClientHandler);

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
                try
                {
                    if (messageEvent.Event == "message")
                    {
                        // Just message as event means a heartbeat/keepalive
                        _listener.OnKeepAlive();
                    }
                    else
                    {
                        // Handle correct message
                        HandleMessage(messageEvent);
                        if (offsetPersister != null) offsetPersister.Offset = long.Parse(messageEvent.Id, CultureInfo.InvariantCulture);
                    }
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    _logger.Error(ex);
                    _listener.OnException(ex);
                }
                _backoffHandler.Reset();
                _timer.Reset();
            }

            void HandleDisconnect(DisconnectEventArgs e)
            {
                _timer.Stop();
                _logger.Debug($"Error: {e.Exception.Message}");
                var persistedOffset = offsetPersister?.Offset ?? initialOffset;
                var recoveredOffset = recoverOffset?.Invoke(persistedOffset) ?? persistedOffset;
                if (_backoffHandler.MaximumNumberOfRetriesUsed())
                {
                    _logger.Error("Reached maximum number of connection retries, stopped consuming event stream.");
                    onFinalDisconnect?.Invoke(e.Exception);
                    return;

                }
                _backoffHandler.WaitBackOffPeriod();

                _logger.Debug("Restarting stream");
                _eventSourceReader = new EventSourceReader(CreateUri(recoveredOffset, confirmationStatus, messageTypes), CoinHttpClientHandler);
                CoinHttpClientHandler.CancellationTokenSource = new CancellationTokenSource();
                _timer.SetToken(CoinHttpClientHandler.CancellationTokenSource);
                StartReading();
            }
        }

        private void HandleMessage(EventSourceMessageEventArgs e)
        {
            var message = JObject.Parse(e.Message).First.First;
            switch (e.Event)
            {
                case "activationsn-v1":
                    _listener.OnActivationServiceNumber(e.Id, message.ToObject<ActivationServiceNumberMessage>());
                    break;
                case "cancel-v1":
                    _listener.OnCancel(e.Id, message.ToObject<CancelMessage>());
                    break;
                case "deactivation-v1":
                    _listener.OnDeactivation(e.Id, message.ToObject<DeactivationMessage>());
                    break;
                case "deactivationsn-v1":
                    _listener.OnDeactivationServiceNumber(e.Id, message.ToObject<DeactivationServiceNumberMessage>());
                    break;
                case "enumactivationnumber-v1":
                    _listener.OnEnumActivationNumber(e.Id, message.ToObject<EnumActivationNumberMessage>());
                    break;
                case "enumactivationoperator-v1":
                    _listener.OnEnumActivationOperator(e.Id, message.ToObject<EnumActivationOperatorMessage>());
                    break;
                case "enumactivationrange-v1":
                    _listener.OnEnumActivationRange(e.Id, message.ToObject<EnumActivationRangeMessage>());
                    break;
                case "enumdeactivationnumber-v1":
                    _listener.OnEnumDeactivationNumber(e.Id, message.ToObject<EnumDeactivationNumberMessage>());
                    break;
                case "enumdeactivationoperator-v1":
                    _listener.OnEnumDeactivationOperator(e.Id, message.ToObject<EnumDeactivationOperatorMessage>());
                    break;
                case "enumdeactivationrange-v1":
                    _listener.OnEnumDeactivationRange(e.Id, message.ToObject<EnumDeactivationRangeMessage>());
                    break;
                case "enumprofileactivation-v1":
                    _listener.OnEnumProfileActivation(e.Id, message.ToObject<EnumProfileActivationMessage>());
                    break;
                case "enumprofiledeactivation-v1":
                    _listener.OnEnumProfileDeactivation(e.Id, message.ToObject<EnumProfileDeactivationMessage>());
                    break;
                case "errorfound-v1":
                    _listener.OnErrorFound(e.Id, message.ToObject<ErrorFoundMessage>());
                    break;
                case "portingperformed-v1":
                    _listener.OnPortingPerformed(e.Id, message.ToObject<PortingPerformedMessage>());
                    break;
                case "portingrequest-v1":
                    _listener.OnPortingRequest(e.Id, message.ToObject<PortingRequestMessage>());
                    break;
                case "portingrequestanswer-v1":
                    _listener.OnPortingRequestAnswer(e.Id, message.ToObject<PortingRequestAnswerMessage>());
                    break;
                case "pradelayed-v1":
                    _listener.OnPortingRequestAnswerDelayed(e.Id, message.ToObject<PortingRequestAnswerDelayedMessage>());
                    break;
                case "rangeactivation-v1":
                    _listener.OnRangeActivation(e.Id, message.ToObject<RangeActivationMessage>());
                    break;
                case "rangedeactivation-v1":
                    _listener.OnRangeDeactivation(e.Id, message.ToObject<RangeDeactivationMessage>());
                    break;
                case "tariffchangesn-v1":
                    _listener.OnTariffChangeServiceNumber(e.Id, message.ToObject<TariffChangeServiceNumberMessage>());
                    break;
                default:
                    _listener.OnUnknownMessage(e.Id, e.Message);
                    break;
            }
        }

        private Uri CreateUri(long offset, ConfirmationStatus confirmationStatus, string[] messageTypes)
        {
            var uri = _sseUri
                .AddQueryArg(nameof(offset), $"{offset}")
                .AddQueryArg(nameof(confirmationStatus), $"{confirmationStatus}");
            if (messageTypes.Length > 0)
                uri.AddQueryArg(nameof(messageTypes), $"{string.Join(",", messageTypes)}");
            return uri;
        }
    }

    internal class ReadTimeOutTimer : IDisposable
    {
        private const int THRESHOLD_TIMEOUT = 300000000;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private long _timestamp = DateTime.Now.Ticks;
        private CancellationTokenSource _cancellationtokensource;

        public ReadTimeOutTimer()
        {
            _timer.Interval = 30000;
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
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

        public void SetToken(CancellationTokenSource cts) => _cancellationtokensource = cts;

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var now = DateTime.Now.Ticks;
            var elapsedTime = now - _timestamp;
            Debug.WriteLine("Timestamp: " + _timestamp);
            Debug.WriteLine("Elapsed Time: " + elapsedTime);

            if (elapsedTime > THRESHOLD_TIMEOUT)
            {
                Debug.WriteLine("Timestamp: " + _timestamp);
                Debug.WriteLine("Time-out above threshold! Quitting: " + e);
                _cancellationtokensource.Cancel();
            }
        }

        #region IDisposable Support
        private bool _disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
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
        private readonly int _backoffperiod;
        private readonly int _numberofretries;
        private int _currentbackoffperiod;
        private int _retriesleft;

        public BackoffHandler(int backOffPeriod, int numberOfRetries)
        {
            _backoffperiod = backOffPeriod;
            _numberofretries = numberOfRetries;
            Reset();
        }

        public int GetBackOffPeriod() => _backoffperiod;

        public int GetRetriesLeft() => _numberofretries;

        public void Reset()
        {
            _currentbackoffperiod = _backoffperiod;
            _retriesleft = _numberofretries;
        }
        public void DecreaseNumberOfRetries()
        {
            if (_retriesleft > 0)
                _retriesleft--;
        }

        private void IncreaseBackOffPeriod() => _currentbackoffperiod = (_currentbackoffperiod > 60) ? _currentbackoffperiod : _currentbackoffperiod * 2;
        public bool MaximumNumberOfRetriesUsed() => _retriesleft <= 0;

        public void WaitBackOffPeriod()
        {
            Debug.WriteLine($"Going to sleep for {_currentbackoffperiod} seconds and still {_retriesleft} retries left!");
            Thread.Sleep(_currentbackoffperiod * 1000);
            IncreaseBackOffPeriod();
            DecreaseNumberOfRetries();
        }
    }
}