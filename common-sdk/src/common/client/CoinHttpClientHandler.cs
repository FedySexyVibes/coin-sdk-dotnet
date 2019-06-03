using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Coin.Common.Crypto;

namespace Coin.Common.Client
{
    public class CoinHttpClientHandler : HttpClientHandler
    {
        readonly CtpApiClientUtil.HmacSignatureType _hmacSignatureType;
        readonly HMACSHA256 _signer;
        readonly RSA _privateKey;
        readonly string _consumerName;
        readonly int _validPeriodInSeconds;

        public CoinHttpClientHandler(string consumerName, HMACSHA256 signer, RSA privateKey,
            CtpApiClientUtil.HmacSignatureType hmacSignatureHmacSignatureType = CtpApiClientUtil.HmacSignatureType.XDateAndDigest, int validPeriodInSeconds = CtpApiClientUtil.DefaultValidPeriodInSecs)
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
            var hmacHeaders = CtpApiClientUtil.GetHmacHeaders(_hmacSignatureType, content);
            foreach (var pair in hmacHeaders)
            {
                request.Headers.Add(pair.Key, pair.Value);
            }
            var requestLine = $"{request.Method} {request.RequestUri.LocalPath} HTTP/1.1";
            request.Headers.Add("authorization", CtpApiClientUtil.CalculateHttpRequestHmac(_signer, _consumerName, hmacHeaders, requestLine));
            
            var jwt = CtpApiClientUtil.CreateJwt(_privateKey, _consumerName, _validPeriodInSeconds);
            CookieContainer = new CookieContainer();
            CookieContainer.Add(request.RequestUri, new Cookie("jwt", jwt));
            return await base.SendAsync(request, cancellationToken);
        }
    }
}