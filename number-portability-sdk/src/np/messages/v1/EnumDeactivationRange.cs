using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class EnumDeactivationRangeMessage: INpMessage<EnumDeactivationRange> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumDeactivationRangeBody>))]
        public INpMessageBody<EnumDeactivationRange> Body { get; set; }
    }

    public class EnumDeactivationRangeBody : INpMessageBody<EnumDeactivationRange> {

        [DataMember(Name="enumdeactivationrange", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "enumdeactivationrange")]
        public EnumDeactivationRange Content { get; set; }
    }

    public class EnumDeactivationRange : INpMessageContent {

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
