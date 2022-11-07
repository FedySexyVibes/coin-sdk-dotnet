using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V5
{
    public interface IMessageEnvelope<out T> where T : IBsMessageContent
    {
        IBsMessage<T> Message { get; }
    }

    public interface IBsMessage<out T> where T : IBsMessageContent
    {
        Header Header { get; set; }
        IBsMessageBody<T> Body { get; }
    }

    public interface IBsMessageBody<out T> where T : IBsMessageContent
    {
        T Content { get; }
    }

    public interface IBsMessageContent
    {
        string DossierId { get; set; }
    }

    public class MessageEnvelope<T> : IMessageEnvelope<T> where T : IBsMessageContent
    {
        [DataMember(Name = "message", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "message")]
        public IBsMessage<T> Message { get; set; }
    }
}