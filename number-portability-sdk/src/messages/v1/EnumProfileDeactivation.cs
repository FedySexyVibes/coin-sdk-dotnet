using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1 {

    public class EnumProfileDeactivationMessage: INpMessage<EnumProfileDeactivation> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<EnumProfileDeactivationBody>))]
        public INpMessageBody<EnumProfileDeactivation> Body { get; set; }
    }

    public class EnumProfileDeactivationBody : INpMessageBody<EnumProfileDeactivation> {

        [DataMember(Name="enumprofiledeactivation", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "enumprofiledeactivation")]
        public EnumProfileDeactivation Content { get; set; }
    }

    public class EnumProfileDeactivation : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="currentnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "currentnetworkoperator")]
        public string CurrentNetworkOperator { get; set; }

        [DataMember(Name="typeofnumber", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "typeofnumber")]
        public string TypeOfNumber { get; set; }

        [DataMember(Name="profileid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "profileid")]
        public string ProfileId { get; set; }
    }
}
