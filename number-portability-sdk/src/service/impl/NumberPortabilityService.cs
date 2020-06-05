using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Coin.Sdk.Common;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using Newtonsoft.Json.Linq;
using static Coin.Sdk.Common.Crypto.CtpApiClientUtil;
using static Coin.Sdk.NP.Messages.V1.Utils;

namespace Coin.Sdk.NP.Service.Impl
{

    public class NumberPortabilityService : CtpApiRestTemplateSupport, INumberPortabilityService
    {
        readonly Uri _apiUrl;

        public NumberPortabilityService(string apiUrl, string consumerName, string privateKeyFile, string encryptedHmacSecretFile)
            : this(new Uri(apiUrl), consumerName, privateKeyFile, encryptedHmacSecretFile) { }

        public NumberPortabilityService(Uri apiUrl, string consumerName, string privateKeyFile, string encryptedHmacSecretFile)
            : this(apiUrl, consumerName, ReadPrivateKeyFile(privateKeyFile), encryptedHmacSecretFile) { }

        public NumberPortabilityService(string apiUrl, string consumerName, RSA privateKey, string encryptedHmacSecretFile)
            : this(new Uri(apiUrl), consumerName, privateKey, encryptedHmacSecretFile) { }

        public NumberPortabilityService(Uri apiUrl, string consumerName, RSA privateKey, string encryptedHmacSecretFile)
            : this(apiUrl, consumerName, HmacFromEncryptedBase64EncodedSecretFile(encryptedHmacSecretFile, privateKey), privateKey) { }

        public NumberPortabilityService(string apiUrl, string consumerName, HMACSHA256 signer, RSA privateKey)
            : this(new Uri(apiUrl), consumerName, signer, privateKey) { }

        public NumberPortabilityService(Uri apiUrl, string consumerName, HMACSHA256 signer, RSA privateKey) : base(consumerName, privateKey, signer) => _apiUrl = apiUrl;

        public Task<HttpResponseMessage> SendConfirmationAsync(string id)
        {
            var confirmationMessage = new ConfirmationMessage { TransactionId = id };
            return SendWithTokenAsync(HttpMethod.Put, _apiUrl.AddPathArg($"dossiers/confirmations/{id}"), confirmationMessage);
        }

        public async Task<MessageResponse> SendMessageAsync(IMessageEnvelope<INpMessageContent> envelope)
        {
            var responseMessage = await SendWithTokenAsync(HttpMethod.Post, _apiUrl.AddPathArg($"dossiers/{TypeName(envelope)}"), envelope).ConfigureAwait(false);
            var responseBody = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            var json = JObject.Parse(responseBody);
            if (responseMessage.IsSuccessStatusCode)
                return json.ToObject<MessageResponse>();
            if (json.TryGetValue("transactionId", out _))
                return json.ToObject<ErrorResponse>();
            throw new HttpListenerException((int)responseMessage.StatusCode, responseBody);
        }
    }
}