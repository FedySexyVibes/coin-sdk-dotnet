using System;
using System.Diagnostics;
using Coin.Sdk.Common.Client;
using Coin.Sdk.BS.Messages.V4;
using Coin.Sdk.BS.Service;

namespace Coin.Sdk.BS.Tests
{
    public static class TestUtils
    {
        public static string GenerateDossierId(string recipient, string donor)
        {
            var randomNumber5Digits = (Math.Round(new Random().Next() * 10000.0) + 9999).ToString();
            return $"{recipient}-{donor}-{randomNumber5Digits}-1";
        }
    }

    public static class TestSettings
    {
        public const string PrivateKeyFile = "../../../../keys/private-key.pem";
        public const string EncryptedHmacSecretFile = "../../../../keys/sharedkey.encrypted";

        public static string ApiUrl =
            "http://" + (Environment.GetEnvironmentVariable("STUB_HOST_AND_PORT") ?? "localhost:8000") +
            "/bundle-switching/v4";

        public static string SseUrl = ApiUrl + "/dossiers/events";

        public const string Consumer = "loadtest-loada";
        public const string Recipient = "LOADA";
        public const string Donor = "LOADB";
        public const string PhoneNumber = "0612345678";
        public static readonly string Timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    public class TestListener : IBundleSwitchingMessageListener
    {
        public void OnCancel(string messageId, CancelMessage message)
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

        public void OnContractTerminationPerformed(string messageId, ContractTerminationPerformedMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnContractTerminationRequest(string messageId, ContractTerminationRequestMessage message)
        {
            Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnContractTerminationRequestAnswer(string messageId, ContractTerminationRequestAnswerMessage message)
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