using System;

namespace Coin.Sdk.NP.Sample
{
    public static class TestUtils {

        public static string GenerateDossierId(string operatorCode)
        {
            var randomNumber5Digits = (Math.Round(new Random().Next() * 10000.0) + 9999).ToString();
            return operatorCode + "-" + randomNumber5Digits;
        }

        public static string CrdbReceiver = "CRDB";

        public static string AllOperatorsReceiver = "ALLO";

        public static string GetPath(string relativePath) => $"../../../{relativePath}";

    }
}