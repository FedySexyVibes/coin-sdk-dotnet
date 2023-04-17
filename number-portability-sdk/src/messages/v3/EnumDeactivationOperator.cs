using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V3
{
    public class EnumDeactivationOperatorMessage : INpMessage<EnumDeactivationOperator>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumDeactivationOperatorBody>))]
        public INpMessageBody<EnumDeactivationOperator> Body { get; set; }
    }

    public class EnumDeactivationOperatorBody : INpMessageBody<EnumDeactivationOperator>
    {
        [DataMember(Name = "enumdeactivationoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enumdeactivationoperator")]
        public EnumDeactivationOperator Content { get; set; }
    }

    public class EnumDeactivationOperator : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "currentnetworkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator { get; set; }

        [DataMember(Name = "typeofnumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "typeofnumber")]
        public string TypeOfNumber { get; set; }

        [DataMember(Name = "repeats", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumOperatorRepeats> Repeats { get; set; }
    }
}