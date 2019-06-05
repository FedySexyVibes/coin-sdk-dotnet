using System.Net.Http;
using System.Threading.Tasks;
using Coin.Sdk.NP.Messages.V1;

namespace Coin.Sdk.NP.Service
{

    public interface INumberPortabilityMessageListener
    {
        void OnMessage(string messageId, INpMessage<INpMessageContent> message);
    }

    public interface INumberPortabilityService {

        Task<HttpResponseMessage> SendConfirmation(string id);

        Task<MessageResponse> SendMessage(IMessageEnvelope<INpMessageContent> message);
    }

    public interface IOffsetPersister {
        long Offset { get; set; }
    }
}