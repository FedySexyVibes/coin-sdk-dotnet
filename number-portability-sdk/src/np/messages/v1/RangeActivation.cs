using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class RangeActivationMessage: INpMessage<RangeActivation> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<RangeActivationBody>))]
        public INpMessageBody<RangeActivation> Body { get; set; }
    }

    public class RangeActivationBody : INpMessageBody<RangeActivation> {

        [DataMember(Name="rangeactivation", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "rangeactivation")]
        public RangeActivation Content { get; set; }
    }

    public class RangeActivation : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="currentnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator;

        [DataMember(Name="optanr", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "optanr")]
        public string OptaNr;

        [DataMember(Name="planneddatetime", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "planneddatetime")]
        public string plannedDatetime;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<RangeRepeats> Repeats;
    }
}
