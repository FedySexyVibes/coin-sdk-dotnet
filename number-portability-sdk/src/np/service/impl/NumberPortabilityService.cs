using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Coin.Common.Client;
using Coin.NP.Messages.V1;
using Newtonsoft.Json;
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

        public NumberPortabilityService(string apiUrl, string consumerName, HMACSHA256 signer, RSA privateKey) : base(consumerName, signer, privateKey)
        {
            _apiUrl = apiUrl;
        }

        public Task sendConfirmation(string id)
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
            return JsonConvert.DeserializeObject<MessageResponse>(responseBody);
        }
    }
}