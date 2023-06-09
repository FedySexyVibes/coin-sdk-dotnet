using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coin.Sdk.NP.Messages.V3
{
    public static class Utils
    {
        public static string TypeName<T>() where T : INpMessageContent, new() => TypeName(new T());

        public static string TypeName(IMessageEnvelope<INpMessageContent> envelope)
        {
            if (envelope is null)
                throw new ArgumentNullException(nameof(envelope));
            return TypeName(envelope.Message.Body.Content);
        }

        public static string TypeName(INpMessage<INpMessageContent> message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            return TypeName(message.Body.Content);
        }

        public static string TypeName(INpMessageBody<INpMessageContent> body)
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));
            return TypeName(body.Content);
        }

        public static string TypeName(INpMessageContent content)
        {
            if (content is null)
                throw new ArgumentNullException(nameof(content));
            switch (content)
            {
                case PortingRequestAnswerDelayed _: return "pradelayed";
                case ActivationServiceNumber _: return "activationsn";
                case DeactivationServiceNumber _: return "deactivationsn";
                case TariffChangeServiceNumber _: return "tariffchangesn";
#pragma warning disable CA1308 // Normalize strings to uppercase
                default: return content.GetType().Name.Split('.').Last().ToLowerInvariant().Replace("message", string.Empty);
#pragma warning restore CA1308 // Normalize strings to uppercase
            }
        }

        public static INpMessage<INpMessageContent> Deserialize(string type, string json)
        {
            var message = JObject.Parse(json).First.First;
            switch (type)
            {
                case "activationsn-v3": return message.ToObject<ActivationServiceNumberMessage>();
                case "cancel-v3": return message.ToObject<CancelMessage>();
                case "deactivation-v3": return message.ToObject<DeactivationMessage>();
                case "deactivationsn-v3": return message.ToObject<DeactivationServiceNumberMessage>();
                case "enumactivationnumber-v3": return message.ToObject<EnumActivationNumberMessage>();
                case "enumactivationoperator-v3": return message.ToObject<EnumActivationOperatorMessage>();
                case "enumactivationrange-v3": return message.ToObject<EnumActivationRangeMessage>();
                case "enumdeactivationnumber-v3": return message.ToObject<EnumDeactivationNumberMessage>();
                case "enumdeactivationoperator-v3": return message.ToObject<EnumDeactivationOperatorMessage>();
                case "enumdeactivationrange-v3": return message.ToObject<EnumDeactivationRangeMessage>();
                case "enumprofileactivation-v3": return message.ToObject<EnumProfileActivationMessage>();
                case "enumprofiledeactivation-v3": return message.ToObject<EnumProfileDeactivationMessage>();
                case "errorfound-v3": return message.ToObject<ErrorFoundMessage>();
                case "portingperformed-v3": return message.ToObject<PortingPerformedMessage>();
                case "portingrequest-v3": return message.ToObject<PortingRequestMessage>();
                case "portingrequestanswer-v3": return message.ToObject<PortingRequestAnswerMessage>();
                case "pradelayed-v3": return message.ToObject<PortingRequestAnswerDelayedMessage>();
                case "rangeactivation-v3": return message.ToObject<RangeActivationMessage>();
                case "rangedeactivation-v3": return message.ToObject<RangeDeactivationMessage>();
                case "tariffchangesn-v3": return message.ToObject<TariffChangeServiceNumberMessage>();
                default: throw new JsonException($"Unknown message type {type}");
            }
        }
    }

    public class ConcreteConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));
            return serializer.Deserialize<T>(reader);
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            if (serializer is null)
                throw new ArgumentNullException(nameof(serializer));
            serializer.Serialize(writer, value);
        }
    }
}