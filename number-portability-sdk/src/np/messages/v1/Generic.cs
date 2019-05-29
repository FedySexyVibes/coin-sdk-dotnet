using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

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

    public class EnumOperatorContent : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="currentnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator;

        [DataMember(Name="typeofnumber", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "typeofnumber")]
        public string TypeOfNumber;

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumOperatorRepeats> Repeats;
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
