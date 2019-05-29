using System;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1
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
                default: return content.GetType().ToString().ToLower().Replace("message", "");
            }
        }

        public static INpMessage<INpMessageContent> Deserialize(string type, string json)
        {
            switch (type)
            {
                case "activationsn": return JsonConvert.DeserializeObject<ActivationServiceNumberMessage>(json);
                case "cancel": return JsonConvert.DeserializeObject<CancelMessage>(json);
                case "deactivation": return JsonConvert.DeserializeObject<DeactivationMessage>(json);
                case "deactivationsn": return JsonConvert.DeserializeObject<DeactivationServiceNumberMessage>(json);
                case "enumactivationnumber": return JsonConvert.DeserializeObject<EnumActivationNumberMessage>(json);
                case "enumactivationoperator": return JsonConvert.DeserializeObject<EnumActivationOperatorMessage>(json);
                case "enumactivationrange": return JsonConvert.DeserializeObject<EnumActivationRangeMessage>(json);
                case "enumdeactivationnumber": return JsonConvert.DeserializeObject<EnumDeactivationNumberMessage>(json);
                case "enumdeactivationoperator": return JsonConvert.DeserializeObject<EnumDeactivationOperatorMessage>(json);
                case "enumdeactivationrange": return JsonConvert.DeserializeObject<EnumDeactivationRangeMessage>(json);
                case "enumprofileactivation": return JsonConvert.DeserializeObject<EnumProfileActivationMessage>(json);
                case "enumprofiledeactivation": return JsonConvert.DeserializeObject<EnumProfileDeactivationMessage>(json);
                case "errorfound": return JsonConvert.DeserializeObject<ErrorFoundMessage>(json);
                case "portingperformed": return JsonConvert.DeserializeObject<PortingPerformedMessage>(json);
                case "portingrequest": return JsonConvert.DeserializeObject<PortingRequestMessage>(json);
                case "portingrequestanswer": return JsonConvert.DeserializeObject<PortingRequestAnswerMessage>(json);
                case "pradelayed": return JsonConvert.DeserializeObject<PortingRequestAnswerDelayedMessage>(json);
                case "rangeactivation": return JsonConvert.DeserializeObject<RangeActivationMessage>(json);
                case "rangedeactivation": return JsonConvert.DeserializeObject<RangeDeactivationMessage>(json);
                case "tariffchangesn": return JsonConvert.DeserializeObject<TariffChangeServiceNumberMessage>(json);
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