using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Coin.Common.Crypto;
using Newtonsoft.Json;

namespace Coin.Common.Client
{
    public abstract class CtpApiRestTemplateSupport : AbstractCtpApiSupport
    {
        readonly HttpClient _httpClient;

        protected CtpApiRestTemplateSupport(
            string consumerName,
            HMACSHA256 signer,
            RSA privateKey,
            CtpApiClientUtil.HmacSignatureType hmacSignatureType = CtpApiClientUtil.HmacSignatureType.XDateAndDigest,
            int validPeriodInSeconds = CtpApiClientUtil.DefaultValidPeriodInSecs) :
            base(consumerName, signer, privateKey, hmacSignatureType, validPeriodInSeconds)
        {
            _httpClient = new HttpClient(new CoinHttpClientHandler(consumerName, signer, privateKey, hmacSignatureType, validPeriodInSeconds));
        }

        public Task<HttpResponseMessage> SendWithToken<T>(HttpMethod method, string url, T content) {
            var request = new HttpRequestMessage(method, url);
            var bodyAsString = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(bodyAsString, Encoding.Default, "application/json");
            return _httpClient.SendAsync(request);
        }
    }
}