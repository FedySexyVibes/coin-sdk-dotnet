using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Coin.Sdk.Common.Crypto
{
    public static class CtpApiClientUtil
    {
        public const int DefaultValidPeriodInSecs = 30;

        public enum HmacSignatureType
        {
            Date,
            XDateAndDigest
        }
        
        public static HMACSHA256 HmacFromEncryptedBase64EncodedSecretFile(string filename, RSA privateKey)
        {
            using (var reader = File.OpenText(filename))
            {
                return HmacFromEncryptedBase64EncodedSecret(reader.ReadToEnd(), privateKey);
            }
        }

        public static HMACSHA256 HmacFromEncryptedBase64EncodedSecret(string encryptedSecret, RSA privateKey)
        {
            if (privateKey is null)
                throw new ArgumentNullException(nameof(privateKey));
            var sharedKey = privateKey.Decrypt(Convert.FromBase64String(encryptedSecret), RSAEncryptionPadding.Pkcs1);
            return new HMACSHA256(sharedKey);
        }

        private const string HmacHeaderFormat = "hmac username=\"{0}\", algorithm=\"hmac-sha256\", headers=\"{1} request-line\", signature=\"{2}\"";

        public static string CreateJwt(RSA privateKey, string consumerName, int validPeriodInSeconds)
        {
            var securityKey = new RsaSecurityKey(privateKey);

            var credentials = new SigningCredentials(securityKey, "RS256");
            var nbf = DateTime.Now.AddSeconds(-30);
            var exp = DateTime.Now.AddSeconds(30 + validPeriodInSeconds);

            var jwtToken = new JwtSecurityToken(
                consumerName,
                notBefore: nbf,
                expires: exp,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(jwtToken);
        }
        
        public static string CalculateHttpRequestHmac(HMACSHA256 signer, string consumerName, Dictionary<string, string>
            headers, string requestLine)
        {
            if (signer is null)
                throw new ArgumentNullException(nameof(signer));
            var message = generateHmacMessage(headers, requestLine);
            var signature = Convert.ToBase64String(signer.ComputeHash(Encoding.UTF8.GetBytes(message)));
            return string.Format(CultureInfo.InvariantCulture, HmacHeaderFormat, consumerName, string.Join(" ", headers.Select(p => p.Key)), signature);
        }

        private static string generateHmacMessage(Dictionary<string, string> headers, string requestLine) =>
            string.Join("\n", headers.Select(p => $"{p.Key}: {p.Value}")) + "\n" + requestLine;

        public static RSA ReadPrivateKeyFile(string path)
        {
            using (var reader = File.OpenText(path))
            {
                var keyPair = (AsymmetricCipherKeyPair) new PemReader(reader).ReadObject();
                var parameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters) keyPair.Private);
                
                return Create(parameters);
            }
        }

        private static RSA Create(RSAParameters parameters)
        {
            var rsa = RSA.Create();
            try
            {
                rsa.ImportParameters(parameters);
                return rsa;
            }
            catch
            {
                rsa.Dispose();
                throw;
            }
        }

        public static Dictionary<string, string> GetHmacHeaders(HmacSignatureType hmacSignatureType, string body = null) =>
            GetHmacHeaders(hmacSignatureType, body == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(body));

        public static Dictionary<string, string> GetHmacHeaders(HmacSignatureType hmacSignatureType, byte[] body)
        {
            var hmacHeaders = new Dictionary<string, string>();
            switch (hmacSignatureType)
            {
                case HmacSignatureType.Date:
                    hmacHeaders["date"] = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);
                    break;
                case HmacSignatureType.XDateAndDigest:
                    using (var sha = SHA256.Create())
                    {
                        hmacHeaders["x-date"] = DateTime.UtcNow.ToString("R", CultureInfo.InvariantCulture);
                        hmacHeaders["digest"] = "SHA-256=" + Convert.ToBase64String(sha.ComputeHash(body ?? Array.Empty<byte>()));
                    }
                    break;
            }
            return hmacHeaders;
        }
    }
}