using System;

namespace Coin.Sdk.BS.Sample
{
    public static class TestUtils {

        public static string GenerateDossierId(string recipient, string donor)
        {
            return $"{recipient}-{donor}-{new Random().Next(10000, 99999)}-1";
        }

        public static string GetPath(string relativePath) => $"../../../{relativePath}";

    }
}