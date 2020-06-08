using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;

namespace Coin.Sdk.Common.Client
{
    public abstract class CtpApiRestTemplateSupport : IDisposable
    {
        protected HttpClient HttpClient { get; private set; }
        protected CoinHttpClientHandler CoinHttpClientHandler { get; private set; }

        protected CtpApiRestTemplateSupport(string consumerName, string privateKeyFile, string encryptedHmacSecretFile)
            : this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile) { }

        CtpApiRestTemplateSupport(string consumerName, RSA privateKey, string encryptedHmacSecretFile)
            : this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey)) { }

        protected CtpApiRestTemplateSupport(string consumerName, RSA privateKey, HMACSHA256 signer,
            int validPeriodInSeconds = DefaultValidPeriodInSecs)
        {
            CoinHttpClientHandler = new CoinHttpClientHandler(consumerName, privateKey, signer, validPeriodInSeconds);
            HttpClient = new HttpClient(CoinHttpClientHandler);
        }

        protected async Task<HttpResponseMessage> SendWithTokenAsync<T>(HttpMethod method, Uri url, T content)
        {
            using (var request = new HttpRequestMessage(method, url))
            {
                if (content != null)
                {
                    var bodyAsString = JsonConvert.SerializeObject(content);
                    request.Content = new StringContent(bodyAsString, Encoding.Default, "application/json");
                }
                return await HttpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        #region IDisposable Support

        bool _disposed; // To detect redundant calls

        protected virtual IEnumerable<IDisposable> DisposableFields => new IDisposable[] { HttpClient, CoinHttpClientHandler };

        void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var disposableField in DisposableFields)
                    {
                        disposableField?.Dispose();
                    }
                }
                _disposed = true;
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