using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1
{
    public class RangeDeactivationMessage : INpMessage<RangeDeactivation>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<RangeDeactivationBody>))]
        public INpMessageBody<RangeDeactivation> Body { get; set; }
    }

    public class RangeDeactivationBody : INpMessageBody<RangeDeactivation>
    {
        [DataMember(Name = "rangedeactivation", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "rangedeactivation")]
        public RangeDeactivation Content { get; set; }
    }

    public class RangeDeactivation : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "currentnetworkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator { get; set; }

        [DataMember(Name = "optanr", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "optanr")]
        public string OptaNr { get; set; }

        [DataMember(Name = "planneddatetime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "planneddatetime")]
        public string PlannedDatetime { get; set; }

        [DataMember(Name = "repeats", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<RangeRepeats> Repeats { get; set; }
    }
}