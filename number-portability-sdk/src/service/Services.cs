using System;
using System.Net.Http;
using System.Threading.Tasks;
using Coin.Sdk.NP.Messages.V1;

namespace Coin.Sdk.NP.Service
{

    public interface INumberPortabilityMessageListener
    {
        void OnKeepAlive();

        void OnException(Exception exception);

        void OnUnknownMessage(string messageId, string message);

        void OnPortingRequest(String messageId, PortingRequestMessage message);

        void OnPortingRequestAnswer(String messageId, PortingRequestAnswerMessage message);

        void OnPortingRequestAnswerDelayed(String messageId, PortingRequestAnswerDelayedMessage message);

        void OnPortingPerformed(String messageId, PortingPerformedMessage message);

        void OnDeactivation(String messageId, DeactivationMessage message);

        void OnCancel(String messageId, CancelMessage message);

        void OnErrorFound(String messageId, ErrorFoundMessage message);

        void OnActivationServiceNumber(String messageId, ActivationServiceNumberMessage message);

        void OnDeactivationServiceNumber(String messageId, DeactivationServiceNumberMessage message);

        void OnTariffChangeServiceNumber(String messageId, TariffChangeServiceNumberMessage message);

        void OnRangeActivation(String messageId, RangeActivationMessage message);

        void OnRangeDeactivation(String messageId, RangeDeactivationMessage message);

        void OnEnumActivationNumber(String messageId, EnumActivationNumberMessage message);

        void OnEnumActivationOperator(String messageId, EnumActivationOperatorMessage message);

        void OnEnumActivationRange(String messageId, EnumActivationRangeMessage message);

        void OnEnumDeactivationNumber(String messageId, EnumDeactivationNumberMessage message);

        void OnEnumDeactivationOperator(String messageId, EnumDeactivationOperatorMessage message);

        void OnEnumDeactivationRange(String messageId, EnumDeactivationRangeMessage message);

        void OnEnumProfileActivation(String messageId, EnumProfileActivationMessage message);

        void OnEnumProfileDeactivation(String messageId, EnumProfileDeactivationMessage message);
    }

    public interface INumberPortabilityService {

        Task<HttpResponseMessage> SendConfirmation(string id);

        Task<MessageResponse> SendMessage(IMessageEnvelope<INpMessageContent> message);
    }

    public interface IOffsetPersister {
        long Offset { get; set; }
    }
}