using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1
{
    public class ErrorFoundMessage : INpMessage<ErrorFound>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<ErrorFoundBody>))]
        public INpMessageBody<ErrorFound> Body { get; set; }
    }

    public class ErrorFoundBody : INpMessageBody<ErrorFound>
    {
        [DataMember(Name = "errorfound", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "errorfound")]
        public ErrorFound Content { get; set; }
    }

    public class ErrorFound : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "repeats", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<ErrorFoundRepeats> Repeats { get; set; }
    }

    public class ErrorFoundRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public ErrorFoundSeq Seq { get; set; }
    }

    public class ErrorFoundSeq
    {
        [DataMember(Name = "phonenumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "phonenumber")]
        public string PhoneNumber { get; set; }

        [DataMember(Name = "errorcode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "errorcode")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "description", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}