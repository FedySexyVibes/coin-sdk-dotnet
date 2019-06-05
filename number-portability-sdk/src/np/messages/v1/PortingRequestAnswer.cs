using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1 {

    public class PortingRequestAnswerMessage: INpMessage<PortingRequestAnswer> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<PortingRequestAnswerBody>))]
        public INpMessageBody<PortingRequestAnswer> Body { get; set; }
    }

    public class PortingRequestAnswerBody : INpMessageBody<PortingRequestAnswer> {

        [DataMember(Name="portingrequestanswer", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "portingrequestanswer")]
        public PortingRequestAnswer Content { get; set; }
    }

    public class PortingRequestAnswer : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="blocking", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "blocking")]
        public string Blocking;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<PortingRequestAnswerRepeats> Repeats;
    }

    public class PortingRequestAnswerRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public PortingRequestAnswerSeq Seq;
    }

    public class PortingRequestAnswerSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries;

        [DataMember(Name="blockingcode", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "blockingcode")]
        public string BlockingCode;

        [DataMember(Name="firstpossibledate", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "firstpossibledate")]
        public string FirstPossibleDate;

        [DataMember(Name="note", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "note")]
        public string Note;

        [DataMember(Name="donornetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "donornetworkoperator")]
        public string DonorNetworkOperator;

        [DataMember(Name="donorserviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "donorserviceprovider")]
        public string DonorServiceProvider;
    }
}
