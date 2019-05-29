using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class DeactivationMessage: INpMessage<Deactivation> {

        [DataMember(Name = "header", EmitDefaultValue = false)] [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)] [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<DeactivationBody>))]
        public INpMessageBody<Deactivation> Body { get; set; }
    }

    public class DeactivationBody : INpMessageBody<Deactivation> {

        [DataMember(Name="deactivation", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "deactivation")]
        public Deactivation Content { get; set; }
    }

    public class Deactivation : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="currentnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator;

        [DataMember(Name="originalnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "originalnetworkoperator")]
        public string OriginalNetworkOperator;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<DeactivationRepeats> Repeats;
    }

    public class DeactivationRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public DeactivationSeq Seq;
    }

    public class DeactivationSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries;
    }
}
