using System;

namespace Coin.Sdk.NP.Sample
{
    public static class TestUtils {

        public static string GenerateDossierId(string operatorCode)
        {
            return operatorCode + "-" + new Random().Next(1000000, 9999999);
        }

        public static string CrdbReceiver = "CRDB";

        public static string GetPath(string relativePath) => $"../../../{relativePath}";

    }
}