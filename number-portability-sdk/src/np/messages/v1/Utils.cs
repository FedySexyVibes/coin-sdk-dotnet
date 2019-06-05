using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Coin.Sdk.NP.Messages.V1
{
    public static class Utils
    {

        public static string TypeName(IMessageEnvelope<INpMessageContent> envelope) => TypeName(envelope.Message.Body.Content);
        public static string TypeName(INpMessage<INpMessageContent> message) => TypeName(message.Body.Content);
        public static string TypeName(INpMessageBody<INpMessageContent> body) => TypeName(body.Content);
        public static string TypeName(INpMessageContent content)
        {
            switch (content)
            {
                case PortingRequestAnswerDelayed _: return "pradelayed";
                case ActivationServiceNumber _: return "activationsn";
                case DeactivationServiceNumber _: return "deactivationsn";
                case TariffChangeServiceNumber _: return "tariffchangesn";
                default: return content.GetType().Name.Split('.').Last().ToLower().Replace("message", "");
            }
        }

        public static INpMessage<INpMessageContent> Deserialize(string type, string json)
        {
            var message = JObject.Parse(json).First.First;
            switch (type)
            {
                case "activationsn-v1": return message.ToObject<ActivationServiceNumberMessage>();
                case "cancel-v1": return message.ToObject<CancelMessage>();
                case "deactivation-v1": return message.ToObject<DeactivationMessage>();
                case "deactivationsn-v1": return message.ToObject<DeactivationServiceNumberMessage>();
                case "enumactivationnumber-v1": return message.ToObject<EnumActivationNumberMessage>();
                case "enumactivationoperator-v1": return message.ToObject<EnumActivationOperatorMessage>();
                case "enumactivationrange-v1": return message.ToObject<EnumActivationRangeMessage>();
                case "enumdeactivationnumber-v1": return message.ToObject<EnumDeactivationNumberMessage>();
                case "enumdeactivationoperator-v1": return message.ToObject<EnumDeactivationOperatorMessage>();
                case "enumdeactivationrange-v1": return message.ToObject<EnumDeactivationRangeMessage>();
                case "enumprofileactivation-v1": return message.ToObject<EnumProfileActivationMessage>();
                case "enumprofiledeactivation-v1": return message.ToObject<EnumProfileDeactivationMessage>();
                case "errorfound-v1": return message.ToObject<ErrorFoundMessage>();
                case "portingperformed-v1": return message.ToObject<PortingPerformedMessage>();
                case "portingrequest-v1": return message.ToObject<PortingRequestMessage>();
                case "portingrequestanswer-v1": return message.ToObject<PortingRequestAnswerMessage>();
                case "pradelayed-v1": return message.ToObject<PortingRequestAnswerDelayedMessage>();
                case "rangeactivation-v1": return message.ToObject<RangeActivationMessage>();
                case "rangedeactivation-v1": return message.ToObject<RangeDeactivationMessage>();
                case "tariffchangesn-v1": return message.ToObject<TariffChangeServiceNumberMessage>();
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
            return serializer.Deserialize<T>(reader);    
        }    
     
        public override void WriteJson(JsonWriter writer,    
            object value, JsonSerializer serializer)    
        {    
            serializer.Serialize(writer, value);    
        }    
    }
}