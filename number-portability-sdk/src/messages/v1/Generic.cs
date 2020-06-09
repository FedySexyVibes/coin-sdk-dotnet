using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1
{
    public class MessageResponse
    {
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; set; }
    }

    public class ErrorResponse : MessageResponse
    {
        [JsonProperty(PropertyName = "errors")]
#pragma warning disable CA2227 // Collection properties should be read only
        public List<ErrorContent> Errors { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only
    }

    public class ErrorContent
    {
        [JsonProperty(PropertyName = "code")] public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }

    public class Header
    {
        [DataMember(Name = "receiver", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "receiver")]
        public Receiver Receiver { get; set; }

        [DataMember(Name = "sender", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "sender")]
        public Sender Sender { get; set; }

        [DataMember(Name = "timestamp", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }
    }

    public class Sender
    {
        [DataMember(Name = "networkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "networkoperator")]
        public string NetworkOperator { get; set; }

        [DataMember(Name = "serviceprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "serviceprovider")]
        public string ServiceProvider { get; set; }
    }

    public class Receiver
    {
        [DataMember(Name = "networkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "networkoperator")]
        public string NetworkOperator { get; set; }

        [DataMember(Name = "serviceprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "serviceprovider")]
        public string ServiceProvider { get; set; }
    }

    public class NumberSeries
    {
        [DataMember(Name = "start", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }

        [DataMember(Name = "end", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }
    }

    public class RangeRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public RangeSeq Seq { get; set; }
    }

    public class RangeSeq
    {
        [DataMember(Name = "numberseries", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries { get; set; }

        [DataMember(Name = "pop", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pop")]
        public string Pop { get; set; }
    }

    public class EnumNumberRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public EnumNumberSeq Seq { get; set; }
    }

    public class EnumNumberSeq
    {
        [DataMember(Name = "numberseries", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries { get; set; }

        [DataMember(Name = "repeats", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumRepeats> Repeats { get; set; }
    }

    public class EnumRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public EnumProfileSeq Seq { get; set; }
    }

    public class EnumProfileSeq
    {
        [DataMember(Name = "profileid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId { get; set; }
    }

    public class EnumOperatorRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public EnumOperatorSeq Seq { get; set; }
    }

    public class EnumOperatorSeq
    {
        [DataMember(Name = "profileid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId { get; set; }

        [DataMember(Name = "defaultservice", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "defaultservice")]
        public string DefaultService { get; set; }
    }
}