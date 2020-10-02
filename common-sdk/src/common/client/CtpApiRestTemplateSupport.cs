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
        protected HttpClient HttpClient { get; }
        protected CoinHttpClientHandler CoinHttpClientHandler { get; }
        protected Encoding Encoding { get; }

        protected CtpApiRestTemplateSupport(string consumerName, string privateKeyFile, string encryptedHmacSecretFile, string privateKeyFilePassword = null, Encoding encoding = null)
            : this(consumerName, ReadPrivateKeyFile(privateKeyFile, privateKeyFilePassword), encryptedHmacSecretFile, encoding) { }

        private CtpApiRestTemplateSupport(string consumerName, RSA privateKey, string encryptedHmacSecretFile, Encoding encoding = null)
            : this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey), DefaultValidPeriodInSecs, encoding) { }

        protected CtpApiRestTemplateSupport(string consumerName, RSA privateKey, HMACSHA256 signer,
            int validPeriodInSeconds = DefaultValidPeriodInSecs, Encoding encoding = null)
        {
            CoinHttpClientHandler = new CoinHttpClientHandler(consumerName, privateKey, signer, validPeriodInSeconds);
            HttpClient = new HttpClient(CoinHttpClientHandler);
            Encoding = encoding ?? Encoding.UTF8;
        }

        protected async Task<HttpResponseMessage> SendWithTokenAsync<T>(HttpMethod method, Uri url, T content = null)
            where T : class
        {
            using (var request = new HttpRequestMessage(method, url))
            {
                if (content != null)
                {
                    var bodyAsString = JsonConvert.SerializeObject(content);
                    request.Content = new StringContent(bodyAsString, Encoding, "application/json");
                }
                return await HttpClient.SendAsync(request).ConfigureAwait(false);
            }
        }

        #region IDisposable Support

        private bool _disposed; // To detect redundant calls

        protected virtual IEnumerable<IDisposable> DisposableFields => new IDisposable[] { HttpClient, CoinHttpClientHandler };

        private void Dispose(bool disposing)
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