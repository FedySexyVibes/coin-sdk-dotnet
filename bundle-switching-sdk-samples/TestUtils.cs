using System;

namespace Coin.Sdk.BS.Sample
{
    public static class TestUtils {

        public static string GenerateDossierId(string recipient, string donor)
        {
            var randomNumber5Digits = (Math.Round(new Random().Next() * 10000.0) + 9999).ToString();
            return $"{recipient}-{donor}-{randomNumber5Digits}-1";
        }

        public static string GetPath(string relativePath) => $"../../../{relativePath}";

    }
}