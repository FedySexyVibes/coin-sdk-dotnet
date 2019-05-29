using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public interface IMessageEnvelope<out T> where T : INpMessageContent
    {
        INpMessage<T> Message { get; }
    }

    public interface INpMessage<out T> where T : INpMessageContent {
        Header Header { get; set; }
        INpMessageBody<T> Body { get; }
    }

    public interface INpMessageBody<out T> where T : INpMessageContent {
        T Content { get; }
    }

    public interface INpMessageContent {
        string DossierId { get; set; }
    }

    public class MessageEnvelope<T>: IMessageEnvelope<T> where T : INpMessageContent {

        [DataMember(Name="message", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "message")]
        public INpMessage<T> Message { get; set; }
    }

    public class Header {

        [DataMember(Name="receiver", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "receiver")]
        public Receiver Receiver;

        [DataMember(Name="sender", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "sender")]
        public Sender Sender;

        [DataMember(Name = "timestamp", EmitDefaultValue = false)] [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp;
    }

    public class Sender {

        [DataMember(Name="networkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "networkoperator")]
        public string NetworkOperator;

        [DataMember(Name="serviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "serviceprovider")]
        public string ServiceProvider;
    }

    public class Receiver {

        [DataMember(Name="networkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "networkoperator")]
        public string NetworkOperator;

        [DataMember(Name="serviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "serviceprovider")]
        public string ServiceProvider;
    }

    public class MessageResponse {

        [DataMember(Name="transactionId", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId;
    }
}