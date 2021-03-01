using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;

namespace Coin.Sdk.Common.Client
{
    public class CoinHttpClientHandler : HttpClientHandler
    {
        private readonly HMACSHA256 _signer;
        private readonly RSA _privateKey;
        private readonly string _consumerName;
        private readonly int _validPeriodInSeconds;

        private static readonly ByteArrayContent EmptyContent = new ByteArrayContent(Array.Empty<byte>());

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public CoinHttpClientHandler(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            int validPeriodInSeconds = DefaultValidPeriodInSecs, string privateKeyFilePassword = null) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile, privateKeyFilePassword), encryptedHmacSecretFile, validPeriodInSeconds)
        { }

        private CoinHttpClientHandler(string consumerName, RSA privateKey, string encryptedHmacSecretFile, int validPeriodInSeconds) :
            this(consumerName, privateKey,
                HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey), validPeriodInSeconds)
        { }

        public CoinHttpClientHandler(string consumerName, RSA privateKey, HMACSHA256 signer, int validPeriodInSeconds = DefaultValidPeriodInSecs)
        {
            _signer = signer;
            _consumerName = consumerName;
            _privateKey = privateKey;
            _validPeriodInSeconds = validPeriodInSeconds;
            UseCookies = true;
        }

        public CoinHttpClientHandler Copy() => new CoinHttpClientHandler(_consumerName, _privateKey, _signer, _validPeriodInSeconds)
        {
            CancellationTokenSource = CancellationTokenSource
        };

        private static async Task<Dictionary<string, string>> GetHmacHeadersAsync(HttpRequestMessage request)
        {
            // In the .NET 4.7 & 4.8 Runtime the implementation throws an exception when a body is added 
            // to the request. The following if statement is added for this reason, otherwise the SSE stream
            // can't be opened. 
            if (request.Method.Equals(HttpMethod.Get))
            {
                return GetHmacHeaders(Array.Empty<byte>());
            }

            request.Content = request.Content ?? EmptyContent;
            var content = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return GetHmacHeaders(content);
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
            var hmacHeaders = await GetHmacHeadersAsync(request).ConfigureAwait(false);
            foreach (var pair in hmacHeaders)
                request.Headers.Add(pair.Key, pair.Value);

            var requestLine = $"{request.Method} {request.RequestUri.LocalPath} HTTP/1.1";
            request.Headers.Add("authorization", CalculateHttpRequestHmac(_signer, _consumerName, hmacHeaders, requestLine));
            request.Headers.Add("User-Agent", SdkInfo.UserAgent);

            var jwt = CreateJwt(_privateKey, _consumerName, _validPeriodInSeconds);
            CookieContainer.Add(request.RequestUri, new Cookie("jwt", jwt));

            var ctsToken = CancellationTokenSource?.Token ?? cancellationToken;
            return await base.SendAsync(request, ctsToken).ConfigureAwait(false);
        }
    }
}