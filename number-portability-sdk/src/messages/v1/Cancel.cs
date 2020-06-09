using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1
{
    public class CancelMessage : INpMessage<Cancel>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<CancelBody>))]
        public INpMessageBody<Cancel> Body { get; set; }
    }

    public class CancelBody : INpMessageBody<Cancel>
    {
        [DataMember(Name = "cancel", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cancel")]
        public Cancel Content { get; set; }
    }

    public class Cancel : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "note", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }
}