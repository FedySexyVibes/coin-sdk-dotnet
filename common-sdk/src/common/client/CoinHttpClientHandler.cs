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
        private readonly HmacSignatureType _hmacSignatureType;
        private readonly HMACSHA256 _signer;
        private readonly RSA _privateKey;
        private readonly string _consumerName;
        private readonly int _validPeriodInSeconds;
        public CancellationTokenSource CancellationTokenSource
        { get; set; }

        public CoinHttpClientHandler(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            HmacSignatureType hmacSignatureHmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile, hmacSignatureHmacSignatureType, validPeriodInSeconds) {}

        private CoinHttpClientHandler(string consumerName, RSA privateKey, string encryptedHmacSecretFile,
            HmacSignatureType hmacSignatureHmacSignatureType, int validPeriodInSeconds) :
            this(consumerName, privateKey, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey),
                hmacSignatureHmacSignatureType, validPeriodInSeconds) {}

        public CoinHttpClientHandler(string consumerName, RSA privateKey, HMACSHA256 signer, 
            HmacSignatureType hmacSignatureHmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs)
        {
            _hmacSignatureType = hmacSignatureHmacSignatureType;
            _signer = signer;
            _consumerName = consumerName;
            _privateKey = privateKey;
            _validPeriodInSeconds = validPeriodInSeconds;
            UseCookies = true;
        }
        
        private async Task<Dictionary<string, string>> getHmacHeaders(HttpRequestMessage request)
        {
            // In the .NET 4.7 & 4.8 Runtime the implementation throws an exception when a body is added 
            // to the request. The following if statement is added for this reason, otherwise the SSE stream
            // can't be opened. 
            if (request.Method.Equals(HttpMethod.Get))
            {
                return GetHmacHeaders(_hmacSignatureType, System.Array.Empty<byte>());
            }
            else
            {
                request.Content = request.Content ?? new ByteArrayContent(System.Array.Empty<byte>());
                var content = await request.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                return GetHmacHeaders(_hmacSignatureType, content);
            }
        }

        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new System.ArgumentNullException(nameof(request));
            var hmacHeaders = await getHmacHeaders(request).ConfigureAwait(false);
            foreach (var pair in hmacHeaders)
            {
                request.Headers.Add(pair.Key, pair.Value);
            }
            var requestLine = $"{request.Method} {request.RequestUri.LocalPath} HTTP/1.1";
            request.Headers.Add("authorization", CalculateHttpRequestHmac(_signer, _consumerName, hmacHeaders, requestLine));
            request.Headers.Add("User-Agent", $"coin-sdk-dotnet-{SdkInfo.UserAgent}");

            var jwt = CreateJwt(_privateKey, _consumerName, _validPeriodInSeconds);
            CookieContainer.Add(request.RequestUri, new Cookie("jwt", jwt));

            var ctsToken = CancellationTokenSource?.Token ?? cancellationToken;
            return await base.SendAsync(request, ctsToken).ConfigureAwait(false);
        }
    }
}