using System;

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
        public const string PrivateKeyFile = "../../../private-key.pem";
        public const string EncryptedHmacSecretFile =  "../../../sharedkey.encrypted";
        
        public const string ApiUrl = "http://api-stub:8443/number-portability/v1";
        public const string SseUrl = ApiUrl + "/dossiers/events";

        public const string Consumer = "loadtest-loada";
        public const string CrdbReceiver = "CRDB";
        public const string AllOperatorsReceiver = "ALLO";
        public const string Operator = "LOADA";
        public const string PhoneNumber = "0612345678";
        public static readonly string Timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
    }
}