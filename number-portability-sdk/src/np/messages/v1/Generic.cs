using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class MessageResponse {

        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId;
    }

    public class ErrorResponse : MessageResponse {

        [JsonProperty(PropertyName = "errors")]
        public List<ErrorContent> Errors;
    }

    public class ErrorContent {

        [JsonProperty(PropertyName = "code")]
        public string Code;
        
        [JsonProperty(PropertyName = "message")]
        public string Message;
    }

    public class Header {

        [DataMember(Name="receiver", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "receiver")]
        public Receiver Receiver;

        [DataMember(Name="sender", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "sender")]
        public Sender Sender;

        [DataMember(Name = "timestamp", EmitDefaultValue = false)] [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp;
    }

    public class Sender {

        [DataMember(Name="networkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "networkoperator")]
        public string NetworkOperator;

        [DataMember(Name="serviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "serviceprovider")]
        public string ServiceProvider;
    }

    public class Receiver {

        [DataMember(Name="networkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "networkoperator")]
        public string NetworkOperator;

        [DataMember(Name="serviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "serviceprovider")]
        public string ServiceProvider;
    }

    public class NumberSeries {

        [DataMember(Name="start", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "start")]
        public string Start;

        [DataMember(Name="end", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "end")]
        public string End;
    }

    public class RangeRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public RangeSeq Seq;
    }

    public class RangeSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries;

        [DataMember(Name="pop", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "pop")]
        public string Pop;
    }

    public class EnumNumberRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public EnumNumberSeq Seq;
    }

    public class EnumNumberSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumRepeats> Repeats;
    }

    public class EnumRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public EnumProfileSeq Seq;
    }
    
    public class EnumProfileSeq {

        [DataMember(Name="profileid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId;
    }

    public class EnumOperatorRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public EnumOperatorSeq Seq;
    }

    public class EnumOperatorSeq {

        [DataMember(Name="profileid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId;

        [DataMember(Name="defaultservice", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "defaultservice")]
        public string DefaultService;
    }
}
