using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Coin.Common.Client;
using Coin.NP.Messages.V1;
using Newtonsoft.Json.Linq;
using static Coin.Common.Crypto.CtpApiClientUtil;
using static Coin.NP.Messages.V1.Utils;

namespace Coin.NP.Service.Impl
{

    public class NumberPortabilityService : CtpApiRestTemplateSupport, INumberPortabilityService
    {

        readonly string _apiUrl;

        public NumberPortabilityService(string apiUrl, string consumerName, string privateKeyFile, string encryptedHmacSecretFile) : this(
            apiUrl, consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile) {}

        public NumberPortabilityService(string apiUrl, string consumerName, RSA privateKey, string encryptedHmacSecretFile) :
            this(apiUrl, consumerName, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey), privateKey) {}

        public NumberPortabilityService(string apiUrl, string consumerName, HMACSHA256 signer, RSA privateKey) : base(consumerName, privateKey, signer)
        {
            _apiUrl = apiUrl;
        }

        public Task<HttpResponseMessage> sendConfirmation(string id)
        {
            var confirmationMessage = new ConfirmationMessage {TransactionId = id};
            var url = $"{_apiUrl}/dossiers/confirmations/{id}";
            return SendWithToken(HttpMethod.Put, url, confirmationMessage);
        }

        public async Task<MessageResponse> SendMessage(IMessageEnvelope<INpMessageContent> envelope)
        {
            var url = $"{_apiUrl}/dossiers/{TypeName(envelope)}";
            var responseMessage = await SendWithToken(HttpMethod.Post, url, envelope);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);
            if (responseMessage.IsSuccessStatusCode)
            {
                return json.ToObject<MessageResponse>();
            }
            if (json.TryGetValue("transactionId", out _))
            {
                return json.ToObject<ErrorResponse>();
            }
            throw new HttpListenerException((int) responseMessage.StatusCode, responseBody);
        }
    }
}