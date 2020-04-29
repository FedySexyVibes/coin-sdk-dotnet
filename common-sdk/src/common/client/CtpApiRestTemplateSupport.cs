using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;
using Newtonsoft.Json;
using System;

namespace Coin.Sdk.Common.Client
{
    public abstract class CtpApiRestTemplateSupport : IDisposable
    {
        protected readonly HttpClient HttpClient;
        protected readonly CoinHttpClientHandler coinHttpClientHandler;

        protected CtpApiRestTemplateSupport(string consumerName, string privateKeyFile, string encryptedHmacSecretFile) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile) {}

        CtpApiRestTemplateSupport(string consumerName, RSA privateKey, string encryptedHmacSecretFile) :
            this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey)) {}
        
        protected CtpApiRestTemplateSupport(string consumerName, RSA privateKey, HMACSHA256 signer, 
            HmacSignatureType hmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs)
        {
            coinHttpClientHandler = new CoinHttpClientHandler(consumerName, privateKey, signer, hmacSignatureType, validPeriodInSeconds);
            HttpClient = new HttpClient(coinHttpClientHandler);
        }

        protected async Task<HttpResponseMessage> SendWithToken<T>(HttpMethod method, string url, T content) {
            using (var request = new HttpRequestMessage(method, url))
            {
                var bodyAsString = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(bodyAsString, Encoding.Default, "application/json");
                return await HttpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    HttpClient?.Dispose();
                    coinHttpClientHandler?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}