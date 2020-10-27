using System;
using System.Linq;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using EvtSource;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Coin.Sdk.NP.Service.Impl
{
    public class NumberPortabilityMessageConsumer
    {
        private const long DefaultOffset = -1;
        private readonly SseConsumer _sseConsumer;
        private readonly ILogger _logger;

        public NumberPortabilityMessageConsumer(SseConsumer sseConsumer, ILogger logger)
        {
            _sseConsumer = sseConsumer;
            _logger = logger;
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
                switch (eventArgs.Event)
                {
                    case "activationsn-v1":
                        listener.OnActivationServiceNumber(eventArgs.Id, GetNpMessage<ActivationServiceNumberMessage>(eventArgs));
                        return true;
                    case "cancel-v1":
                        listener.OnCancel(eventArgs.Id, GetNpMessage<CancelMessage>(eventArgs));
                        return true;
                    case "deactivation-v1":
                        listener.OnDeactivation(eventArgs.Id, GetNpMessage<DeactivationMessage>(eventArgs));
                        return true;
                    case "deactivationsn-v1":
                        listener.OnDeactivationServiceNumber(eventArgs.Id, GetNpMessage<DeactivationServiceNumberMessage>(eventArgs));
                        return true;
                    case "enumactivationnumber-v1":
                        listener.OnEnumActivationNumber(eventArgs.Id, GetNpMessage<EnumActivationNumberMessage>(eventArgs));
                        return true;
                    case "enumactivationoperator-v1":
                        listener.OnEnumActivationOperator(eventArgs.Id, GetNpMessage<EnumActivationOperatorMessage>(eventArgs));
                        return true;
                    case "enumactivationrange-v1":
                        listener.OnEnumActivationRange(eventArgs.Id, GetNpMessage<EnumActivationRangeMessage>(eventArgs));
                        return true;
                    case "enumdeactivationnumber-v1":
                        listener.OnEnumDeactivationNumber(eventArgs.Id, GetNpMessage<EnumDeactivationNumberMessage>(eventArgs));
                        return true;
                    case "enumdeactivationoperator-v1":
                        listener.OnEnumDeactivationOperator(eventArgs.Id, GetNpMessage<EnumDeactivationOperatorMessage>(eventArgs));
                        return true;
                    case "enumdeactivationrange-v1":
                        listener.OnEnumDeactivationRange(eventArgs.Id, GetNpMessage<EnumDeactivationRangeMessage>(eventArgs));
                        return true;
                    case "enumprofileactivation-v1":
                        listener.OnEnumProfileActivation(eventArgs.Id, GetNpMessage<EnumProfileActivationMessage>(eventArgs));
                        return true;
                    case "enumprofiledeactivation-v1":
                        listener.OnEnumProfileDeactivation(eventArgs.Id, GetNpMessage<EnumProfileDeactivationMessage>(eventArgs));
                        return true;
                    case "errorfound-v1":
                        listener.OnErrorFound(eventArgs.Id, GetNpMessage<ErrorFoundMessage>(eventArgs));
                        return true;
                    case "portingperformed-v1":
                        listener.OnPortingPerformed(eventArgs.Id, GetNpMessage<PortingPerformedMessage>(eventArgs));
                        return true;
                    case "portingrequest-v1":
                        listener.OnPortingRequest(eventArgs.Id, GetNpMessage<PortingRequestMessage>(eventArgs));
                        return true;
                    case "portingrequestanswer-v1":
                        listener.OnPortingRequestAnswer(eventArgs.Id, GetNpMessage<PortingRequestAnswerMessage>(eventArgs));
                        return true;
                    case "pradelayed-v1":
                        listener.OnPortingRequestAnswerDelayed(eventArgs.Id, GetNpMessage<PortingRequestAnswerDelayedMessage>(eventArgs));
                        return true;
                    case "rangeactivation-v1":
                        listener.OnRangeActivation(eventArgs.Id, GetNpMessage<RangeActivationMessage>(eventArgs));
                        return true;
                    case "rangedeactivation-v1":
                        listener.OnRangeDeactivation(eventArgs.Id, GetNpMessage<RangeDeactivationMessage>(eventArgs));
                        return true;
                    case "tariffchangesn-v1":
                        listener.OnTariffChangeServiceNumber(eventArgs.Id, GetNpMessage<TariffChangeServiceNumberMessage>(eventArgs));
                        return true;
                    default:
                        if (string.IsNullOrWhiteSpace(eventArgs.Message))
                        {
                            listener.OnKeepAlive();
                            return true;
                        }
                        listener.OnUnknownMessage(eventArgs.Id, eventArgs.Message);
                        return true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                _logger.LogError(ex, @"An error occured");
                listener.OnException(ex);
                return false;
            }
        }

        private static T GetNpMessage<T>(EventSourceMessageEventArgs eventArgs)
        {
            return JObject.Parse(eventArgs.Message).First.First.ToObject<T>();
        }
    }
}