using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static Coin.Common.Crypto.CtpApiClientUtil;

namespace Coin.Common.Client
{
    public abstract class CtpApiRestTemplateSupport
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

        public Task<HttpResponseMessage> SendWithToken<T>(HttpMethod method, string url, T content) {
            var request = new HttpRequestMessage(method, url);
            var bodyAsString = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(bodyAsString, Encoding.Default, "application/json");
            return HttpClient.SendAsync(request);
        }
    }
}