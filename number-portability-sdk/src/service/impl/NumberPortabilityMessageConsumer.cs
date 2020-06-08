using System;
using System.Linq;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using EvtSource;
using Newtonsoft.Json.Linq;
using NLog;

namespace Coin.Sdk.NP.Service.Impl
{
    public class NumberPortabilityMessageConsumer
    {
        private const long DefaultOffset = -1;
        private readonly SseConsumer _sseConsumer;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public NumberPortabilityMessageConsumer(SseConsumer sseConsumer)
        {
            _sseConsumer = sseConsumer;
        }

        public void StopConsuming()
        {
            _sseConsumer.StopConsuming();
        }

        /// <summary>
        /// Recommended method for consuming messages. On connect or reconnect it will consume all unconfirmed messages.
        /// </summary>
        public void StartConsumingUnconfirmed(
            INumberPortabilityMessageListener listener,
            Action<Exception> onFinalDisconnect = null,
            params MessageType[] messageTypes
        )
        {
            _sseConsumer.StartConsumingUnconfirmed(
                sse => HandleSse(listener, sse),
                messageTypes.Select(m => m.Name()),
                onFinalDisconnect: onFinalDisconnect);
        }

        /// <summary>
        /// Consume all messages, both confirmed and unconfirmed, from a certain offset.
        /// Only use for special cases if <see cref="StartConsumingUnconfirmed"/> does not meet needs.
        /// </summary>
        public void StartConsumingAll(
            INumberPortabilityMessageListener listener,
            IOffsetPersister offsetPersister,
            long offset = DefaultOffset,
            Action<Exception> onFinalDisconnect = null,
            params MessageType[] messageTypes
        )
        {
            _sseConsumer.StartConsumingAll(
                sse => HandleSse(listener, sse),
                offsetPersister,
                offset,
                messageTypes.Select(m => m.Name()),
                onFinalDisconnect: onFinalDisconnect);
        }

        /// <summary>
        /// Only use this method for receiving unconfirmed messages if you make sure that all messages that are received
        /// through this method will be confirmed otherwise, ideally in the stream opened by
        /// <see cref="StartConsumingUnconfirmed"/>. So this method should only be used for a secondary
        /// stream (e.g. backoffice process) that needs to consume unconfirmed messages for administrative purposes.
        /// </summary>
        public void StartConsumingUnconfirmedWithOffsetPersistence(
            INumberPortabilityMessageListener listener,
            IOffsetPersister offsetPersister,
            long offset = DefaultOffset,
            Action<Exception> onFinalDisconnect = null,
            params MessageType[] messageTypes
        )
        {
            _sseConsumer.StartConsumingUnconfirmedWithOffsetPersistence(
                sse => HandleSse(listener, sse),
                offsetPersister,
                offset,
                messageTypes.Select(m => m.Name()),
                onFinalDisconnect: onFinalDisconnect);
        }

        private bool HandleSse(INumberPortabilityMessageListener listener, EventSourceMessageEventArgs eventArgs)
        {
            try
            {
                var message = JObject.Parse(eventArgs.Message).First.First;
                switch (eventArgs.Event)
                {
                    case "activationsn-v1":
                        listener.OnActivationServiceNumber(eventArgs.Id,
                            message.ToObject<ActivationServiceNumberMessage>());
                        return true;
                    case "cancel-v1":
                        listener.OnCancel(eventArgs.Id, message.ToObject<CancelMessage>());
                        return true;
                    case "deactivation-v1":
                        listener.OnDeactivation(eventArgs.Id, message.ToObject<DeactivationMessage>());
                        return true;
                    case "deactivationsn-v1":
                        listener.OnDeactivationServiceNumber(eventArgs.Id,
                            message.ToObject<DeactivationServiceNumberMessage>());
                        return true;
                    case "enumactivationnumber-v1":
                        listener.OnEnumActivationNumber(eventArgs.Id, message.ToObject<EnumActivationNumberMessage>());
                        return true;
                    case "enumactivationoperator-v1":
                        listener.OnEnumActivationOperator(eventArgs.Id,
                            message.ToObject<EnumActivationOperatorMessage>());
                        return true;
                    case "enumactivationrange-v1":
                        listener.OnEnumActivationRange(eventArgs.Id, message.ToObject<EnumActivationRangeMessage>());
                        return true;
                    case "enumdeactivationnumber-v1":
                        listener.OnEnumDeactivationNumber(eventArgs.Id,
                            message.ToObject<EnumDeactivationNumberMessage>());
                        return true;
                    case "enumdeactivationoperator-v1":
                        listener.OnEnumDeactivationOperator(eventArgs.Id,
                            message.ToObject<EnumDeactivationOperatorMessage>());
                        return true;
                    case "enumdeactivationrange-v1":
                        listener.OnEnumDeactivationRange(eventArgs.Id,
                            message.ToObject<EnumDeactivationRangeMessage>());
                        return true;
                    case "enumprofileactivation-v1":
                        listener.OnEnumProfileActivation(eventArgs.Id,
                            message.ToObject<EnumProfileActivationMessage>());
                        return true;
                    case "enumprofiledeactivation-v1":
                        listener.OnEnumProfileDeactivation(eventArgs.Id,
                            message.ToObject<EnumProfileDeactivationMessage>());
                        return true;
                    case "errorfound-v1":
                        listener.OnErrorFound(eventArgs.Id, message.ToObject<ErrorFoundMessage>());
                        return true;
                    case "portingperformed-v1":
                        listener.OnPortingPerformed(eventArgs.Id, message.ToObject<PortingPerformedMessage>());
                        return true;
                    case "portingrequest-v1":
                        listener.OnPortingRequest(eventArgs.Id, message.ToObject<PortingRequestMessage>());
                        return true;
                    case "portingrequestanswer-v1":
                        listener.OnPortingRequestAnswer(eventArgs.Id, message.ToObject<PortingRequestAnswerMessage>());
                        return true;
                    case "pradelayed-v1":
                        listener.OnPortingRequestAnswerDelayed(eventArgs.Id,
                            message.ToObject<PortingRequestAnswerDelayedMessage>());
                        return true;
                    case "rangeactivation-v1":
                        listener.OnRangeActivation(eventArgs.Id, message.ToObject<RangeActivationMessage>());
                        return true;
                    case "rangedeactivation-v1":
                        listener.OnRangeDeactivation(eventArgs.Id, message.ToObject<RangeDeactivationMessage>());
                        return true;
                    case "tariffchangesn-v1":
                        listener.OnTariffChangeServiceNumber(eventArgs.Id,
                            message.ToObject<TariffChangeServiceNumberMessage>());
                        return true;
                    default:
                        listener.OnUnknownMessage(eventArgs.Id, eventArgs.Message);
                        return true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning disable CA1031 // Do not catch general exception types
            {
                _logger.Error(ex);
                listener.OnException(ex);
                return false;
            }
        }
    }
}