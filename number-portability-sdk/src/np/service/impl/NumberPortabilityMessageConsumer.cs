using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading;
using Coin.Common.Client;
using Coin.NP.Messages.V1;
using EvtSource;
using NLog;
using static Coin.Common.Crypto.CtpApiClientUtil;
using static Coin.NP.Messages.V1.Utils;

namespace Coin.NP.Service.Impl
{
    public class NumberPortabilityMessageConsumer : CtpApiRestTemplateSupport
    {
        const long DefaultOffset = -1;
        readonly INumberPortabilityMessageListener _listener;
        readonly string _sseUri;
        readonly int? _numberOfRetries;
        readonly int _backOffPeriod;
        readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NumberPortabilityMessageConsumer(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            INumberPortabilityMessageListener listener, string sseUri, int backOffPeriod, int? numberOfRetries = null,
            HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile,
                listener, sseUri, backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds) {}

        NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, string encryptedHmacSecretFile, INumberPortabilityMessageListener listener,
            string sseUri, int backOffPeriod, int? numberOfRetries, HmacSignatureType hmacSignatureType, int validPeriodInSeconds) :
            this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey),
                listener, sseUri, backOffPeriod, numberOfRetries, hmacSignatureType, validPeriodInSeconds) {}
        
        public NumberPortabilityMessageConsumer(string consumerName, RSA privateKey, HMACSHA256 signer, INumberPortabilityMessageListener listener,
            string sseUri, int backOffPeriod, int? numberOfRetries = null, HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest,
            int validPeriodInSeconds = DefaultValidPeriodInSecs) : base(consumerName, privateKey, signer, hmacSignatureType, validPeriodInSeconds)
        {
            _listener = listener;
            _sseUri = sseUri;
            _backOffPeriod = backOffPeriod;
            _numberOfRetries = numberOfRetries;
        }

        public void StartConsuming(
            ConfirmationStatus confirmationStatus = ConfirmationStatus.Unconfirmed,
            long initialOffset = DefaultOffset,
            IOffsetPersister offsetPersister = null,
            Func<long, long> recoverOffset = null,
            params string[] messageTypes)
        {
            if (confirmationStatus == ConfirmationStatus.All && offsetPersister == null) {
                throw new InvalidEnumArgumentException("offsetPersister should be given when confirmationStatus equals All");
            }
            var retriesLeft = _numberOfRetries;
            StartReading(initialOffset);

            void StartReading(long offset)
            {
                var eventSourceReader = new EventSourceReader(CreateUri(offset, confirmationStatus, messageTypes), coinHttpClientHandler);
                eventSourceReader.MessageReceived += (sender, e) => HandleEvent(e);
                eventSourceReader.Start();
                eventSourceReader.Disconnected += (sender, e) => HandleDisconnect(eventSourceReader, e);
                _logger.Info("Stream started");
            }
            
            void HandleEvent(EventSourceMessageEventArgs e)
            {
                try
                {
                    var message = Deserialize(e.Event, e.Message);
                    _listener.OnMessage(e.Id, message);
                    if (offsetPersister != null) offsetPersister.Offset = long.Parse(e.Id);
                    retriesLeft = _numberOfRetries;
                }
                catch(Exception ex)
                {
                    _logger.Error(ex);
                }
            }

            void HandleDisconnect(EventSourceReader reader, DisconnectEventArgs e)
            {
                _logger.Debug($"Error: {e.Exception.Message}");
                _logger.Debug("Restarting stream");
                reader.Dispose();
                var persistedOffset = offsetPersister?.Offset ?? initialOffset;
                var recoveredOffset = recoverOffset?.Invoke(persistedOffset) ?? persistedOffset;
                if (retriesLeft != null && retriesLeft-- == 0)
                {
                    _logger.Info("Reached maximum number of connection retries, stopped consuming event stream.");
                    return;
                }
                Thread.Sleep(_backOffPeriod * 1000);
                StartReading(recoveredOffset);
            }
        }

        Uri CreateUri(long offset, ConfirmationStatus confirmationStatus, string[] messageTypes) =>
            new Uri($"{_sseUri}?offset={offset}&confirmationStatus={confirmationStatus}"
                    + (messageTypes.Length == 0 ? "" : $"&messageTypes={string.Join(",", messageTypes)}"));
    }
}