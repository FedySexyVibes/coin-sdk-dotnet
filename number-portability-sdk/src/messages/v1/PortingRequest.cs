using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V1 {

    public class PortingRequestMessage: INpMessage<PortingRequest> {

        [DataMember(Name="header", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name="body", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<PortingRequestBody>))]
        public INpMessageBody<PortingRequest> Body { get; set; }
    }

    public class PortingRequestBody : INpMessageBody<PortingRequest> {

        [DataMember(Name="portingrequest", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "portingrequest")]
        public PortingRequest Content { get; set; }
    }

    public class PortingRequest : INpMessageContent {

        [DataMember(Name="dossierid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name="recipientserviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "recipientserviceprovider")]
        public string RecipientServiceProvider { get; set; }

        [DataMember(Name="recipientnetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "recipientnetworkoperator")]
        public string RecipientNetworkOperator { get; set; }

        [DataMember(Name="donornetworkoperator", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "donornetworkoperator")]
        public string DonorNetworkOperator { get; set; }

        [DataMember(Name="donorserviceprovider", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "donorserviceprovider")]
        public string DonorServiceProvider { get; set; }

        [DataMember(Name="customerinfo", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "customerinfo")]
        public CustomerInfo CustomerInfo { get; set; }

        [DataMember(Name="note", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<PortingRequestRepeats> Repeats { get; set; }
    }

    public class CustomerInfo {

        [DataMember(Name="lastname", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "lastname")]
        public string Lastname { get; set; }

        [DataMember(Name="companyname", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "companyname")]
        public string Companyname { get; set; }

        [DataMember(Name="housenr", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "housenr")]
        public string HouseNr { get; set; }

        [DataMember(Name="housenrext", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "housenrext")]
        public string HouseNrExt { get; set; }

        [DataMember(Name="postcode", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }

        [DataMember(Name="customerid", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "customerid")]
        public string CustomerId { get; set; }
    }

    public class PortingRequestRepeats {

        [DataMember(Name="seq", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "seq")]
        public PortingRequestSeq Seq { get; set; }
    }

    public class PortingRequestSeq {

        [DataMember(Name="numberseries", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "numberseries")]
        public NumberSeries NumberSeries { get; set; }

        [DataMember(Name="repeats", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<EnumRepeats> Repeats { get; set; }
    }
}
