using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V3
{
    public class EnumActivationOperatorMessage : INpMessage<EnumActivationOperator>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumActivationOperatorBody>))]
        public INpMessageBody<EnumActivationOperator> Body { get; set; }
    }

    public class EnumActivationOperatorBody : INpMessageBody<EnumActivationOperator>
    {
        [DataMember(Name = "enumactivationoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enumactivationoperator")]
        public EnumActivationOperator Content { get; set; }
    }

    public class EnumActivationOperator : INpMessageContent
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