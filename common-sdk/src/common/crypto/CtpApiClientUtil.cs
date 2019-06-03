using System;
using System.Collections.Generic;
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

namespace Coin.Common.Crypto
{
    public class CtpApiClientUtil
    {
        public const int DefaultValidPeriodInSecs = 30;

        CtpApiClientUtil() {}

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
            var sharedKey = privateKey.Decrypt(Convert.FromBase64String(encryptedSecret), RSAEncryptionPadding.Pkcs1);
            return new HMACSHA256(sharedKey);
        }

        const string HmacHeaderFormat = "hmac username=\"{0}\", algorithm=\"hmac-sha256\", headers=\"{1} request-line\", signature=\"{2}\"";

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
            var message = generateHmacMessage(headers, requestLine);
            var signature = Convert.ToBase64String(signer.ComputeHash(Encoding.UTF8.GetBytes(message)));
            return string.Format(HmacHeaderFormat, consumerName, string.Join(" ", headers.Select(p => p.Key)), signature);
        }
        
        static string generateHmacMessage(Dictionary<string, string> headers, string requestLine)
        {
            return string.Join("\n", headers.Select(p => $"{p.Key}: {p.Value}")) + "\n" + requestLine;
        }

        public static RSA ReadPrivateKeyFile(string path)
        {
            using (var reader = File.OpenText(path))
            {
                var keyPair = (AsymmetricCipherKeyPair) new PemReader(reader).ReadObject();
                var parameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters) keyPair.Private);
                
                return Create(parameters);
            }
        }
        
        static RSA Create(RSAParameters parameters)
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

        public static Dictionary<string, string> GetHmacHeaders(HmacSignatureType hmacSignatureType, string body = null)
        {
            return GetHmacHeaders(hmacSignatureType, body == null ? new byte[0] : Encoding.UTF8.GetBytes(body));
        }

        public static Dictionary<string, string> GetHmacHeaders(HmacSignatureType hmacSignatureType, byte[] body)
        {
            var hmacHeaders = new Dictionary<string, string>();
            switch (hmacSignatureType)
            {
                case HmacSignatureType.Date:
                    hmacHeaders["date"] = DateTime.UtcNow.ToString("R");
                    break;
                case HmacSignatureType.XDateAndDigest:
                    hmacHeaders["x-date"] = DateTime.UtcNow.ToString("R");
                    hmacHeaders["digest"] = "SHA-256=" + Convert.ToBase64String(SHA256.Create().ComputeHash(body ?? new byte[0]));
                    break;
            }
            return hmacHeaders;
        }
    }
}