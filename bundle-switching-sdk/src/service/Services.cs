using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coin.Sdk.BS.Messages.V4;

namespace Coin.Sdk.BS.Service
{

    public interface IBundleSwitchingMessageListener
    {
        void OnKeepAlive();

        void OnException(Exception exception);

        void OnUnknownMessage(string messageId, string message);

        void OnContractTerminationRequest(string messageId, ContractTerminationRequestMessage message);

        void OnContractTerminationRequestAnswer(string messageId, ContractTerminationRequestAnswerMessage message);

        void OnContractTerminationPerformed(string messageId, ContractTerminationPerformedMessage message);

        void OnCancel(string messageId, CancelMessage message);

        void OnErrorFound(string messageId, ErrorFoundMessage message);
    }

    public interface IBundleSwitchingService {

        Task<HttpResponseMessage> SendConfirmationAsync(string id);

        Task<MessageResponse> SendMessageAsync(IMessageEnvelope<IBsMessageContent> message);
    }
}