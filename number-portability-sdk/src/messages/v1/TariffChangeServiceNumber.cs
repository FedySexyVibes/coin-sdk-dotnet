using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1 {

    public class TariffChangeServiceNumberMessage: INpMessage<TariffChangeServiceNumber> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<TariffChangeServiceNumberBody>))]
        public INpMessageBody<TariffChangeServiceNumber> Body { get; set; }
    }

    public class TariffChangeServiceNumberBody : INpMessageBody<TariffChangeServiceNumber> {

        [DataMember(Name="tariffchangesn", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "tariffchangesn")]
        public TariffChangeServiceNumber Content { get; set; }
    }

    public class TariffChangeServiceNumber : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="platformprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "platformprovider")]
        public string PlatformProvider { get; set; }

        [DataMember(Name="planneddatetime", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "planneddatetime")]
        public string PlannedDatetime { get; set; }

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<TariffChangeServiceNumberRepeats> Repeats { get; set; }
    }

    public class TariffChangeServiceNumberRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public TariffChangeServiceNumberSeq Seq { get; set; }
    }

    public class TariffChangeServiceNumberSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries { get; set; }

        [DataMember(Name="tariffinfonew", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "tariffinfonew")]
        public TariffInfo TariffInfoNew { get; set; }
    }

    public class TariffInfo {

        [DataMember(Name="peak", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "peak")]
        public string Peak { get; set; }

        [DataMember(Name="offpeak", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "offpeak")]
        public string OffPeak { get; set; }

        [DataMember(Name="currency", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [DataMember(Name="type", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [DataMember(Name="vat", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "vat")]
        public string Vat { get; set; }
    }
}
