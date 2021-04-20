using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V3
{
    public interface IMessageEnvelope<out T> where T : INpMessageContent
    {
        INpMessage<T> Message { get; }
    }

    public interface INpMessage<out T> where T : INpMessageContent
    {
        Header Header { get; set; }
        INpMessageBody<T> Body { get; }
    }

    public interface INpMessageBody<out T> where T : INpMessageContent
    {
        T Content { get; }
    }

    public interface INpMessageContent
    {
        string DossierId { get; set; }
    }

    public class MessageEnvelope<T> : IMessageEnvelope<T> where T : INpMessageContent
    {
        [DataMember(Name = "message", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "message")]
        public INpMessage<T> Message { get; set; }
    }
}