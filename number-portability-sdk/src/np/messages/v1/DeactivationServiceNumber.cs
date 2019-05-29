using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class DeactivationServiceNumberMessage: INpMessage<DeactivationServiceNumber> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<DeactivationServiceNumberBody>))]
        public INpMessageBody<DeactivationServiceNumber> Body { get; set; }
    }

    public class DeactivationServiceNumberBody : INpMessageBody<DeactivationServiceNumber> {

        [DataMember(Name="deactivationsn", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "deactivationsn")]
        public DeactivationServiceNumber Content { get; set; }
    }

    public class DeactivationServiceNumber : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="platformprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "platformprovider")]
        public string PlatformProvider;

        [DataMember(Name="planneddatetime", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "planneddatetime")]
        public string PlannedDatetime;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<DeactivationServiceNumberRepeats> Repeats;
    }

    public class DeactivationServiceNumberRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public DeactivationServiceNumberSeq Seq;
    }

    public class DeactivationServiceNumberSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries;

        [DataMember(Name="pop", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "pop")]
        public string Pop;
    }
}
