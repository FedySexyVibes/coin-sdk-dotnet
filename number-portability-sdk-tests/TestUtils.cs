using System;
using System.Collections.Generic;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service;

namespace Coin.Sdk.NP.Tests
{
    public static class TestUtils {

        public static string GenerateDossierId(string operatorCode)
        {
            var randomNumber5Digits = (Math.Round(new Random().Next() * 10000.0) + 9999).ToString();
            return operatorCode + "-" + randomNumber5Digits;
        }
    }
    
    public static class TestSettings
    {
        public const string PrivateKeyFile = "../../../keys/private-key.pem";
        public const string EncryptedHmacSecretFile =  "../../../keys/sharedkey.encrypted";
        
        public static string ApiUrl = (Environment.GetEnvironmentVariable("STUB_HOST_AND_PORT") ?? "http://localhost:8000") + "/number-portability/v1";
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
        public List<(string messageId, INpMessage<INpMessageContent> message)> Messages;
        public void Clear() => Messages = new List<(string messageId, INpMessage<INpMessageContent> message)>();

        public void OnMessage(string messageId, INpMessage<INpMessageContent> message) =>
            Messages.Add((messageId, message));
    }

    public class TestOffsetPersister : IOffsetPersister
    {
        public long Offset { get; set; }
    }
}