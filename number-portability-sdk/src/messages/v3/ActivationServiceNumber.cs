using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V3
{
    public class ActivationServiceNumberMessage : INpMessage<ActivationServiceNumber>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<ActivationServiceNumberBody>))]
        public INpMessageBody<ActivationServiceNumber> Body { get; set; }
    }

    public class ActivationServiceNumberBody : INpMessageBody<ActivationServiceNumber>
    {
        [DataMember(Name = "activationsn", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "activationsn")]
        public ActivationServiceNumber Content { get; set; }
    }

    public class ActivationServiceNumber : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "platformprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "platformprovider")]
        public string PlatformProvider { get; set; }

        [DataMember(Name = "planneddatetime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "planneddatetime")]
        public string PlannedDatetime { get; set; }

        [DataMember(Name = "note", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [DataMember(Name = "repeats", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<ActivationServiceNumberRepeats> Repeats { get; set; }
    }

    public class ActivationServiceNumberRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public ActivationServiceNumberSeq Seq { get; set; }
    }

    public class ActivationServiceNumberSeq
    {
        [DataMember(Name = "numberseries", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries { get; set; }

        [DataMember(Name = "tariffinfo", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "tariffinfo")]
        public TariffInfo TariffInfo { get; set; }

        [DataMember(Name = "pop", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pop")]
        public string Pop { get; set; }
    }
}