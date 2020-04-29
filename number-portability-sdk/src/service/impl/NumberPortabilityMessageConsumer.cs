using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.Common.Client;
using EvtSource;
using NLog;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;
using System.Timers;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Coin.Sdk.src.common;

namespace Coin.Sdk.NP.Service.Impl
{
    public class NumberPortabilityMessageConsumer : CtpApiRestTemplateSupport
    {
        private const long DefaultOffset = -1;
        private readonly INumberPortabilityMessageListener _listener;
        private readonly Uri _sseUri;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private EventSourceReader _eventSourceReader;
        private ReadTimeOutTimer _timer = new ReadTimeOutTimer();
        private BackoffHandler _backoffHandler;

        public NumberPortabilityMessageConsumer(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            INumberPortabilityMessageListener listener, string sseUri, int backOffPeriod = 1, int numberOfRetries = 3,
            HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile,
                listener, new Uri(sseUri), backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds) { }

        public NumberPortabilityMessageConsumer(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            INumberPortabilityMessageListener listener, Uri sseUri, int backOffPeriod = 1, int numberOfRetries = 3,
            HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile,
                listener, sseUri, backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds)
        { }

        private NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, string encryptedHmacSecretFile, INumberPortabilityMessageListener listener,
            Uri sseUri, int backOffPeriod, int numberOfRetries, HmacSignatureType hmacSignatureType, int validPeriodInSeconds) :
            this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey),
                listener, sseUri, backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds) { }

        public NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, HMACSHA256 signer, INumberPortabilityMessageListener listener,
            string sseUri, int backOffPeriod = 1, int numberOfRetries = 3, HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest,
            int validPeriodInSeconds = DefaultValidPeriodInSecs) 
            : this(consumerName, privateKey, signer, listener,
                  new Uri(sseUri), backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds) {}

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
            System.Diagnostics.Debug.WriteLine("Quitting because testcase ended!");
            _timer.Stop();
            if (_eventSourceReader?.IsDisposed == false) _eventSourceReader.Dispose();
        }

        public void StartConsuming(
            ConfirmationStatus confirmationStatus = ConfirmationStatus.Unconfirmed,
            long initialOffset = DefaultOffset,
            IOffsetPersister offsetPersister = null,
            Func<long, long> recoverOffset = null,
            Action<Exception> onFinalDisconnect = null,
            params string[] messageTypes)
        {
            if (confirmationStatus == ConfirmationStatus.All && offsetPersister == null) {
                throw new InvalidEnumArgumentException("offsetPersister should be given when confirmationStatus equals All");
            }

            coinHttpClientHandler.CancellationTokenSource = new CancellationTokenSource();
            _timer.SetToken(coinHttpClientHandler.CancellationTokenSource);
            _eventSourceReader = new EventSourceReader(CreateUri(initialOffset, confirmationStatus, messageTypes), coinHttpClientHandler);

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
                try { 
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
                catch (Exception ex)
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
                _eventSourceReader = new EventSourceReader(CreateUri(recoveredOffset, confirmationStatus, messageTypes), coinHttpClientHandler);
                coinHttpClientHandler.CancellationTokenSource = new CancellationTokenSource();
                _timer.SetToken(coinHttpClientHandler.CancellationTokenSource);
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
        private const int THRESHOLD_TIME_OUT = 300000000;
        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private long timestamp = DateTime.Now.Ticks;
        private CancellationTokenSource cancellationTokenSource;

        public ReadTimeOutTimer()
        {
            timer.Interval = 30000;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        public void UpdateTimestamp()
        {
            timestamp = DateTime.Now.Ticks;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            timer.Stop();
            timestamp = DateTime.Now.Ticks;
            timer.Start();
        }

        public void SetToken(CancellationTokenSource cts)
        {
            cancellationTokenSource = cts;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var now = DateTime.Now.Ticks;
            var elapsedTime = now - timestamp;
            System.Diagnostics.Debug.WriteLine("Timestamp: " + timestamp);
            System.Diagnostics.Debug.WriteLine("Elapsed Time: " + elapsedTime);

            if (elapsedTime > THRESHOLD_TIME_OUT)
            {
                System.Diagnostics.Debug.WriteLine("Timestamp: " + timestamp);
                System.Diagnostics.Debug.WriteLine("Time-out above threshold! Quitting: " + e);
                cancellationTokenSource.Cancel();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer?.Dispose();
                }
                disposedValue = true;
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
        private int backOffPeriod;
        private int numberOfRetries;
        private int currentBackOffPeriod;
        private int retriesLeft;

        private long timestamp
        { get; set; }

        public BackoffHandler(int backOffPeriod, int numberOfRetries)
        {
            this.backOffPeriod = backOffPeriod;
            this.numberOfRetries = numberOfRetries;
            Reset();
        }

        public int GetBackOffPeriod()
        {
            return backOffPeriod;
        }

        public int GetRetriesLeft()
        {
            return numberOfRetries;
        }

        public void Reset()
        {
            currentBackOffPeriod = backOffPeriod;
            retriesLeft = numberOfRetries;
            timestamp = DateTime.Now.Ticks;
        }
        public void DecreaseNumberOfRetries()
        {
            if (retriesLeft > 0)
                retriesLeft--;
        }

        private void IncreaseBackOffPeriod()
        {
            currentBackOffPeriod = (currentBackOffPeriod > 60) ? currentBackOffPeriod : currentBackOffPeriod * 2;
        }
        public bool MaximumNumberOfRetriesUsed()
        {
            return retriesLeft <= 0; 
        }

        public void WaitBackOffPeriod()
        {
            System.Diagnostics.Debug.WriteLine($"Going to sleep for {currentBackOffPeriod} seconds and still {retriesLeft} retries left!");
            Thread.Sleep(currentBackOffPeriod * 1000);
            IncreaseBackOffPeriod();
            DecreaseNumberOfRetries();
        }
    }
}