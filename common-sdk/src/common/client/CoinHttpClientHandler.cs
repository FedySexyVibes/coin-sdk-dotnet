using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using static Coin.Common.Crypto.CtpApiClientUtil;

namespace Coin.Common.Client
{
    public class CoinHttpClientHandler : HttpClientHandler
    {
        readonly HmacSignatureType _hmacSignatureType;
        readonly HMACSHA256 _signer;
        readonly RSA _privateKey;
        readonly string _consumerName;
        readonly int _validPeriodInSeconds;

        public CoinHttpClientHandler(string consumerName, string privateKeyFile, string encryptedHmacSecretFile,
            HmacSignatureType hmacSignatureHmacSignatureType = HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = DefaultValidPeriodInSecs) :
            this(consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile, hmacSignatureHmacSignatureType, validPeriodInSeconds) {}

        CoinHttpClientHandler(string consumerName, RSA privateKey, string encryptedHmacSecretFile,
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
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Content = request.Content ?? new ByteArrayContent(new byte[0]);
            var content = await request.Content.ReadAsByteArrayAsync();
            var hmacHeaders = GetHmacHeaders(_hmacSignatureType, content);
            foreach (var pair in hmacHeaders)
            {
                request.Headers.Add(pair.Key, pair.Value);
            }
            var requestLine = $"{request.Method} {request.RequestUri.LocalPath} HTTP/1.1";
            request.Headers.Add("authorization", CalculateHttpRequestHmac(_signer, _consumerName, hmacHeaders, requestLine));
            
            var jwt = CreateJwt(_privateKey, _consumerName, _validPeriodInSeconds);
            CookieContainer = new CookieContainer();
            CookieContainer.Add(request.RequestUri, new Cookie("jwt", jwt));
            return await base.SendAsync(request, cancellationToken);
        }
    }
}