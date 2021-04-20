using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography;
using Coin.Sdk.Common;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V3;
using Coin.Sdk.NP.Service;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;

namespace Coin.Sdk.NP.Tests
{
    public static class TestUtils
    {
        public static string GenerateDossierId(string operatorCode)
        {
            var randomNumber5Digits = (Math.Round(new Random().Next() * 10000.0) + 9999).ToString();
            return operatorCode + "-" + randomNumber5Digits;
        }
    }

    public static class TestSettings
    {
        public const string PrivateKeyFile = "../../../../keys/private-key.pem";
        public const string EncryptedHmacSecretFile = "../../../../keys/sharedkey.encrypted";
        public const int NumberOfRetries = 1;

        public static readonly string ApiUrl =
            "http://" + (Environment.GetEnvironmentVariable("STUB_HOST_AND_PORT") ?? "localhost:8000") +
            "/number-portability/v3";

        public static readonly string SseUrl = ApiUrl + "/dossiers/events";

        public const string Consumer = "loadtest-loada";
        public const string CrdbReceiver = "CRDB";
        public const string Operator = "LOADA";
        public const string PhoneNumber = "0612345678";
        public static readonly string Timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
    }

    public class StopStreamService : CtpApiRestTemplateSupport
    {
        private readonly Uri _apiUrl;

        public StopStreamService(string apiUrl, string consumerName, string privateKeyFile,
            string encryptedHmacSecretFile, string? privateKeyFilePassword = null)
            : this(new Uri(apiUrl), consumerName, ReadPrivateKeyFile(privateKeyFile, privateKeyFilePassword),
                encryptedHmacSecretFile)
        {
        }

        public StopStreamService(Uri apiUrl, string consumerName, RSA privateKey, string encryptedHmacSecretFile)
            : this(apiUrl, consumerName, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey),
                privateKey)
        {
        }

        public StopStreamService(Uri apiUrl, string consumerName, HMACSHA256 signer, RSA privateKey) : base(
            consumerName, privateKey, signer) => _apiUrl = apiUrl;
        
        public void StopStream()
        {
            SendWithTokenAsync(HttpMethod.Get, _apiUrl.AddPathArg("dossiers/stopstream"), "");
        }
    }

    public class TestListener : INumberPortabilityMessageListener
    {
        private Action<string> _sideEffect = _ => { };

        public Action<string> SideEffect
        {
            set => _sideEffect = value;
        }

        public void OnActivationServiceNumber(string messageId, ActivationServiceNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnCancel(string messageId, CancelMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnDeactivation(string messageId, DeactivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnDeactivationServiceNumber(string messageId, DeactivationServiceNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumActivationNumber(string messageId, EnumActivationNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumActivationOperator(string messageId, EnumActivationOperatorMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumActivationRange(string messageId, EnumActivationRangeMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumDeactivationNumber(string messageId, EnumDeactivationNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumDeactivationOperator(string messageId, EnumDeactivationOperatorMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumDeactivationRange(string messageId, EnumDeactivationRangeMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumProfileActivation(string messageId, EnumProfileActivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnEnumProfileDeactivation(string messageId, EnumProfileDeactivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnErrorFound(string messageId, ErrorFoundMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnException(Exception exception)
        {
            Debug.WriteLine($"Exception received {exception}");
        }

        public void OnKeepAlive()
        {
            Debug.WriteLine("Keepalive");
        }

        public void OnPortingPerformed(string messageId, PortingPerformedMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnPortingRequest(string messageId, PortingRequestMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnPortingRequestAnswer(string messageId, PortingRequestAnswerMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnPortingRequestAnswerDelayed(string messageId, PortingRequestAnswerDelayedMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnRangeActivation(string messageId, RangeActivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnRangeDeactivation(string messageId, RangeDeactivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnTariffChangeServiceNumber(string messageId, TariffChangeServiceNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
            _sideEffect(messageId);
        }

        public void OnUnknownMessage(string messageId, string message)
        {
            Debug.WriteLine($"Unknown message received: {message}");
        }
    }

    public class TestOffsetPersister : IOffsetPersister
    {
        public long Offset { get; set; }
    }
}