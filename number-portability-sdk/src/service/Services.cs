using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coin.Sdk.NP.Messages.V3;

namespace Coin.Sdk.NP.Service
{

    public interface INumberPortabilityMessageListener
    {
        void OnKeepAlive();

        void OnException(Exception exception);

        void OnUnknownMessage(string messageId, string message);

        void OnPortingRequest(string messageId, PortingRequestMessage message);

        void OnPortingRequestAnswer(string messageId, PortingRequestAnswerMessage message);

        void OnPortingRequestAnswerDelayed(string messageId, PortingRequestAnswerDelayedMessage message);

        void OnPortingPerformed(string messageId, PortingPerformedMessage message);

        void OnDeactivation(string messageId, DeactivationMessage message);

        void OnCancel(string messageId, CancelMessage message);

        void OnErrorFound(string messageId, ErrorFoundMessage message);

        void OnActivationServiceNumber(string messageId, ActivationServiceNumberMessage message);

        void OnDeactivationServiceNumber(string messageId, DeactivationServiceNumberMessage message);

        void OnTariffChangeServiceNumber(string messageId, TariffChangeServiceNumberMessage message);

        void OnRangeActivation(string messageId, RangeActivationMessage message);

        void OnRangeDeactivation(string messageId, RangeDeactivationMessage message);

        void OnEnumActivationNumber(string messageId, EnumActivationNumberMessage message);

        void OnEnumActivationOperator(string messageId, EnumActivationOperatorMessage message);

        void OnEnumActivationRange(string messageId, EnumActivationRangeMessage message);

        void OnEnumDeactivationNumber(string messageId, EnumDeactivationNumberMessage message);

        void OnEnumDeactivationOperator(string messageId, EnumDeactivationOperatorMessage message);

        void OnEnumDeactivationRange(string messageId, EnumDeactivationRangeMessage message);

        void OnEnumProfileActivation(string messageId, EnumProfileActivationMessage message);

        void OnEnumProfileDeactivation(string messageId, EnumProfileDeactivationMessage message);
    }

    public interface INumberPortabilityService {

        Task<HttpResponseMessage> SendConfirmationAsync(string id);

        Task<MessageResponse> SendMessageAsync(IMessageEnvelope<INpMessageContent> message);
    }
}