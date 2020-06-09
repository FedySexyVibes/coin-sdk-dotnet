using System;
using System.Collections.Generic;
using System.Linq;
using Coin.Sdk.Common.Client;
using Coin.Sdk.BS.Messages.V4;
using EvtSource;
using Newtonsoft.Json.Linq;
using NLog;

namespace Coin.Sdk.BS.Service.Impl
{
    public class BundleSwitchingMessageConsumer
    {
        private const long DefaultOffset = -1;
        private readonly SseConsumer _sseConsumer;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public BundleSwitchingMessageConsumer(SseConsumer sseConsumer)
        {
            _sseConsumer = sseConsumer;
        }

        public void StopConsuming()
        {
            _sseConsumer.StopConsuming();
        }

        /// <summary>
        /// Recommended method for consuming messages. On connect or reconnect it will consume all unconfirmed messages.
        /// <br/><br/>
        /// Only provide serviceProviders if there are multiple service providers linked to your consumer
        /// and you only want to consume messages for a subset of these providers.
        /// </summary>
        public void StartConsumingUnconfirmed(
            IBundleSwitchingMessageListener listener,
            Action<Exception> onFinalDisconnect = null,
            IEnumerable<string> serviceProviders = null,
            params MessageType[] messageTypes
        )
        {
            _sseConsumer.StartConsumingUnconfirmed(
                sse => HandleSse(listener, sse),
                messageTypes.Select(m => m.Name()),
                new Dictionary<string, IEnumerable<string>> {["serviceprovider"] = serviceProviders},
                onFinalDisconnect);
        }

        /// <summary>
        /// Consume all messages, both confirmed and unconfirmed, from a certain offset.
        /// Only use for special cases if <see cref="StartConsumingUnconfirmed"/> does not meet needs.
        /// <br/><br/>
        /// Only provide serviceProviders if there are multiple service providers linked to your consumer
        /// and you only want to consume messages for a subset of these providers.
        /// </summary>
        public void StartConsumingAll(
            IBundleSwitchingMessageListener listener,
            IOffsetPersister offsetPersister,
            long offset = DefaultOffset,
            Action<Exception> onFinalDisconnect = null,
            IEnumerable<string> serviceProviders = null,
            params MessageType[] messageTypes
        )
        {
            _sseConsumer.StartConsumingAll(
                sse => HandleSse(listener, sse),
                offsetPersister,
                offset,
                messageTypes.Select(m => m.Name()),
                new Dictionary<string, IEnumerable<string>> {["serviceprovider"] = serviceProviders},
                onFinalDisconnect);
        }

        /// <summary>
        /// Only use this method for receiving unconfirmed messages if you make sure that all messages that are received
        /// through this method will be confirmed otherwise, ideally in the stream opened by
        /// <see cref="StartConsumingUnconfirmed"/>. So this method should only be used for a secondary
        /// stream (e.g. backoffice process) that needs to consume unconfirmed messages for administrative purposes.
        /// <br/><br/>
        /// Only provide serviceProviders if there are multiple service providers linked to your consumer
        /// and you only want to consume messages for a subset of these providers.
        /// </summary>
        public void StartConsumingUnconfirmedWithOffsetPersistence(
            IBundleSwitchingMessageListener listener,
            IOffsetPersister offsetPersister,
            long offset = DefaultOffset,
            Action<Exception> onFinalDisconnect = null,
            IEnumerable<string> serviceProviders = null,
            params MessageType[] messageTypes
        )
        {
            _sseConsumer.StartConsumingUnconfirmedWithOffsetPersistence(
                sse => HandleSse(listener, sse),
                offsetPersister,
                offset,
                messageTypes.Select(m => m.Name()),
                new Dictionary<string, IEnumerable<string>> {["serviceprovider"] = serviceProviders},
                onFinalDisconnect);
        }

        private bool HandleSse(IBundleSwitchingMessageListener listener, EventSourceMessageEventArgs eventArgs)
        {
            try
            {
                var message = JObject.Parse(eventArgs.Message).First.First;
                switch (eventArgs.Event)
                {
                    case "cancel-v4":
                        listener.OnCancel(eventArgs.Id, message.ToObject<CancelMessage>());
                        return true;
                    case "errorfound-v4":
                        listener.OnErrorFound(eventArgs.Id, message.ToObject<ErrorFoundMessage>());
                        return true;
                    case "contractterminationperformed-v4":
                        listener.OnContractTerminationPerformed(eventArgs.Id, message.ToObject<ContractTerminationPerformedMessage>());
                        return true;
                    case "contractterminationrequest-v4":
                        listener.OnContractTerminationRequest(eventArgs.Id, message.ToObject<ContractTerminationRequestMessage>());
                        return true;
                    case "contractterminationrequestanswer-v4":
                        listener.OnContractTerminationRequestAnswer(eventArgs.Id, message.ToObject<ContractTerminationRequestAnswerMessage>());
                        return true;
                    default:
                        listener.OnUnknownMessage(eventArgs.Id, eventArgs.Message);
                        return true;
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                _logger.Error(ex);
                listener.OnException(ex);
                return false;
            }
        }
    }
}