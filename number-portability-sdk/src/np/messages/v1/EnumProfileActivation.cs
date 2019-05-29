using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class EnumProfileActivationMessage: INpMessage<EnumProfileActivation> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumProfileActivationBody>))]
        public INpMessageBody<EnumProfileActivation> Body { get; set; }
    }

    public class EnumProfileActivationBody : INpMessageBody<EnumProfileActivation> {

        [DataMember(Name="enumprofileactivation", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "enumprofileactivation")]
        public EnumProfileActivation Content { get; set; }
    }

    public class EnumProfileActivation : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="currentnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator;

        [DataMember(Name="typeofnumber", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "typeofnumber")]
        public string TypeOfNumber;

        [DataMember(Name="scope", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "scope")]
        public string Scope;

        [DataMember(Name="profileid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId;

        [DataMember(Name="ttl", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "ttl")]
        public string Ttl;

        [DataMember(Name="dnsclass", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dnsclass")]
        public string DnsClass;

        [DataMember(Name="rectype", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "rectype")]
        public string RecType;

        [DataMember(Name="order", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "order")]
        public string Order;

        [DataMember(Name="preference", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "preference")]
        public string Preference;

        [DataMember(Name="flags", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "flags")]
        public string Flags;

        [DataMember(Name="enumservice", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "enumservice")]
        public string EnumService;

        [DataMember(Name="regexp", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "regexp")]
        public string Regexp;

        [DataMember(Name="usertag", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "usertag")]
        public string UserTag;

        [DataMember(Name="domain", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "domain")]
        public string Domain;

        [DataMember(Name="spcode", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "spcode")]
        public string SpCode;

        [DataMember(Name="processtype", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "processtype")]
        public string ProcessType;

        [DataMember(Name="gateway", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "gateway")]
        public string Gateway;

        [DataMember(Name="service", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "service")]
        public string Service;

        [DataMember(Name="domaintag", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "domaintag")]
        public string DomainTag;

        [DataMember(Name="replacement", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "replacement")]
        public string Replacement;
    }
}
