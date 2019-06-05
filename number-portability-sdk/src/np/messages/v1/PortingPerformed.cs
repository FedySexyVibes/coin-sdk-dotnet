using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1 {

    public class PortingPerformedMessage: INpMessage<PortingPerformed> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<PortingPerformedBody>))]
        public INpMessageBody<PortingPerformed> Body { get; set; }
    }

    public class PortingPerformedBody : INpMessageBody<PortingPerformed> {

        [DataMember(Name="portingperformed", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "portingperformed")]
        public PortingPerformed Content { get; set; }
    }
    public class PortingPerformed : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="recipientnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "recipientnetworkoperator")]
        public string RecipientNetworkOperator;

        [DataMember(Name="donornetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "donornetworkoperator")]
        public string DonorNetworkOperator;

        [DataMember(Name="actualdatetime", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "actualdatetime")]
        public string ActualDatetime;

        [DataMember(Name="batchporting", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "batchporting")]
        public string BatchPorting;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<PortingPerformedRepeats> Repeats;
    }

    public class PortingPerformedRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public PortingPerformedSeq Seq;
    }

    public class PortingPerformedSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries;

        [DataMember(Name="backporting", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "backporting")]
        public string BackPorting;

        [DataMember(Name="pop", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "pop")]
        public string Pop;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumRepeats> Repeats;
    }
}
