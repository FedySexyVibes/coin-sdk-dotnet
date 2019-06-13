using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1 {

    public class EnumActivationNumberMessage: INpMessage<EnumActivationNumber> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumActivationNumberBody>))]
        public INpMessageBody<EnumActivationNumber> Body { get; set; }
    }

    public class EnumActivationNumberBody : INpMessageBody<EnumActivationNumber> {

        [DataMember(Name="enumactivationnumber", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "enumactivationnumber")]
        public EnumActivationNumber Content { get; set; }
    }

    public class EnumActivationNumber : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="currentnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator;

        [DataMember(Name="typeofnumber", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "typeofnumber")]
        public string TypeOfNumber;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumNumberRepeats> Repeats;
    }
}
