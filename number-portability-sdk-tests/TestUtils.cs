using System;
using System.Diagnostics;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service;

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
        public const string PrivateKeyFile = "../../../keys/private-key.pem";
        public const string EncryptedHmacSecretFile = "../../../keys/sharedkey.encrypted";

        public static string ApiUrl =
            "http://" + (Environment.GetEnvironmentVariable("STUB_HOST_AND_PORT") ?? "localhost:8000") +
            "/number-portability/v1";

        public static string SseUrl = ApiUrl + "/dossiers/events";

        public const string Consumer = "loadtest-loada";
        public const string CrdbReceiver = "CRDB";
        public const string AllOperatorsReceiver = "ALLO";
        public const string Operator = "LOADA";
        public const string PhoneNumber = "0612345678";
        public static readonly string Timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
    }

    public class TestListener : INumberPortabilityMessageListener
    {
        public void OnActivationServiceNumber(string messageId, ActivationServiceNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnCancel(string messageId, CancelMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnDeactivation(string messageId, DeactivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnDeactivationServiceNumber(string messageId, DeactivationServiceNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumActivationNumber(string messageId, EnumActivationNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumActivationOperator(string messageId, EnumActivationOperatorMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumActivationRange(string messageId, EnumActivationRangeMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumDeactivationNumber(string messageId, EnumDeactivationNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumDeactivationOperator(string messageId, EnumDeactivationOperatorMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumDeactivationRange(string messageId, EnumDeactivationRangeMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumProfileActivation(string messageId, EnumProfileActivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumProfileDeactivation(string messageId, EnumProfileDeactivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnErrorFound(string messageId, ErrorFoundMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
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
        }

        public void OnPortingRequest(string messageId, PortingRequestMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnPortingRequestAnswer(string messageId, PortingRequestAnswerMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnPortingRequestAnswerDelayed(string messageId, PortingRequestAnswerDelayedMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnRangeActivation(string messageId, RangeActivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnRangeDeactivation(string messageId, RangeDeactivationMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnTariffChangeServiceNumber(string messageId, TariffChangeServiceNumberMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
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