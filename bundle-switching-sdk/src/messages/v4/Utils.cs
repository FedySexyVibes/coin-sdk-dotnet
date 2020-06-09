using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coin.Sdk.BS.Messages.V4
{
    public static class Utils
    {
        public static string TypeName<T>() where T : IBsMessageContent, new() => TypeName(new T());

        public static string TypeName(IMessageEnvelope<IBsMessageContent> envelope)
        {
            if (envelope is null)
                throw new ArgumentNullException(nameof(envelope));
            return TypeName(envelope.Message.Body.Content);
        }

        public static string TypeName(IBsMessage<IBsMessageContent> message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            return TypeName(message.Body.Content);
        }

        public static string TypeName(IBsMessageBody<IBsMessageContent> body)
        {
            if (body is null)
                throw new ArgumentNullException(nameof(body));
            return TypeName(body.Content);
        }

        public static string TypeName(IBsMessageContent content)
        {
            if (content is null)
                throw new ArgumentNullException(nameof(content));
#pragma warning disable CA1308 // Normalize strings to uppercase
            return content.GetType().Name.Split('.').Last().ToLowerInvariant().Replace("message", string.Empty);
#pragma warning restore CA1308 // Normalize strings to uppercase
        }

        public static IBsMessage<IBsMessageContent> Deserialize(string type, string json)
        {
            var message = JObject.Parse(json).First.First;
            switch (type)
            {
                case "cancel-v4": return message.ToObject<CancelMessage>();
                case "errorfound-v4": return message.ToObject<ErrorFoundMessage>();
                case "contractterminationperformed-v4": return message.ToObject<ContractTerminationPerformedMessage>();
                case "contractterminationrequest-v4": return message.ToObject<ContractTerminationRequestMessage>();
                case "contractterminationrequestanswer-v4":
                    return message.ToObject<ContractTerminationRequestAnswerMessage>();
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