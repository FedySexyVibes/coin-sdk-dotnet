using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V3
{
    public class EnumProfileActivationMessage : INpMessage<EnumProfileActivation>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumProfileActivationBody>))]
        public INpMessageBody<EnumProfileActivation> Body { get; set; }
    }

    public class EnumProfileActivationBody : INpMessageBody<EnumProfileActivation>
    {
        [DataMember(Name = "enumprofileactivation", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enumprofileactivation")]
        public EnumProfileActivation Content { get; set; }
    }

    public class EnumProfileActivation : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "currentnetworkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator { get; set; }

        [DataMember(Name = "typeofnumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "typeofnumber")]
        public string TypeOfNumber { get; set; }

        [DataMember(Name = "scope", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        [DataMember(Name = "profileid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId { get; set; }

        [DataMember(Name = "ttl", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "ttl")]
        public string Ttl { get; set; }

        [DataMember(Name = "dnsclass", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dnsclass")]
        public string DnsClass { get; set; }

        [DataMember(Name = "rectype", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "rectype")]
        public string RecType { get; set; }

        [DataMember(Name = "order", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }

        [DataMember(Name = "preference", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "preference")]
        public string Preference { get; set; }

        [DataMember(Name = "flags", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "flags")]
        public string Flags { get; set; }

        [DataMember(Name = "enumservice", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "enumservice")]
        public string EnumService { get; set; }

        [DataMember(Name = "regexp", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "regexp")]
        public string Regexp { get; set; }

        [DataMember(Name = "usertag", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "usertag")]
        public string UserTag { get; set; }

        [DataMember(Name = "domain", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "domain")]
        public string Domain { get; set; }

        [DataMember(Name = "spcode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "spcode")]
        public string SpCode { get; set; }

        [DataMember(Name = "processtype", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "processtype")]
        public string ProcessType { get; set; }

        [DataMember(Name = "gateway", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        [DataMember(Name = "service", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "service")]
        public string Service { get; set; }

        [DataMember(Name = "domaintag", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "domaintag")]
        public string DomainTag { get; set; }

        [DataMember(Name = "replacement", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "replacement")]
        public string Replacement { get; set; }
    }
}