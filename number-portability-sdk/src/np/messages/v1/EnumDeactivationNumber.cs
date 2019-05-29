using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class EnumDeactivationNumberMessage: INpMessage<EnumDeactivationNumber> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumDeactivationNumberBody>))]
        public INpMessageBody<EnumDeactivationNumber> Body { get; set; }
    }

    public class EnumDeactivationNumberBody : INpMessageBody<EnumDeactivationNumber> {

        [DataMember(Name="enumdeactivationnumber", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "enumdeactivationnumber")]
        public EnumDeactivationNumber Content { get; set; }
    }

    public class EnumDeactivationNumber : INpMessageContent {

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
