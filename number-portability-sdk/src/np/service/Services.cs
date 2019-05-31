using System.Threading.Tasks;
using Coin.NP.Messages.V1;

namespace Coin.NP.Service
{

    public interface INumberPortabilityMessageListener
    {
        void OnMessage(string messageId, INpMessage<INpMessageContent> message);
    }

    public interface INumberPortabilityService {

        Task sendConfirmation(string id);

        Task<MessageResponse> SendMessage(IMessageEnvelope<INpMessageContent> message);
    }
    
    public interface IOffsetPersister {
        long Offset { get; set; }
    }
}