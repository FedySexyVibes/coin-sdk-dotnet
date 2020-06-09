using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V4
{
    public class ContractTerminationRequestMessage : IBsMessage<ContractTerminationRequest>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<ContractTerminationRequestBody>))]
        public IBsMessageBody<ContractTerminationRequest> Body { get; set; }
    }

    public class ContractTerminationRequestBody : IBsMessageBody<ContractTerminationRequest>
    {
        [DataMember(Name = "contractterminationrequest", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "contractterminationrequest")]
        public ContractTerminationRequest Content { get; set; }
    }

    public class ContractTerminationRequest : IBsMessageContent
    {
        /// <summary>
        /// The identifier for the Contract Termination Request determined by the Recipient for communication between the Recipient and Donor. Defined as [RecipientSPCode]-[DonorSPCode]-[Identifier] The requirement is that the dossierid is unique and identifies the contract termination request. 
        /// </summary>
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        /// <summary>
        /// Code of the Recipient party
        /// </summary>
        [DataMember(Name = "recipientserviceprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recipientserviceprovider")]
        public string RecipientServiceProvider { get; set; }

        /// <summary>
        /// Code of the Recipient party
        /// </summary>
        [DataMember(Name = "recipientnetworkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recipientnetworkoperator")]
        public string RecipientNetworkOperator { get; set; }

        /// <summary>
        /// Code of the Donor party
        /// </summary>
        [DataMember(Name = "donornetworkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "donornetworkoperator")]
        public string DonorNetworkOperator { get; set; }

        /// <summary>
        /// Code of the Donor party 
        /// </summary>
        [DataMember(Name = "donorserviceprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "donorserviceprovider")]
        public string DonorServiceProvider { get; set; }

        /// <summary>
        /// Indicates whether the dossier is a business request or not
        /// </summary>
        [DataMember(Name = "business", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "business")]
        public string Business { get; set; }

        /// <summary>
        /// Indicates whether there is an authorisation for early termination of contract
        /// </summary>
        [DataMember(Name = "earlytermination", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "earlytermination")]
        public string EarlyTermination { get; set; }

        /// <summary>
        /// Contract name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DataMember(Name = "addressblock", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "addressblock")]
        public AddressBlock AddressBlock { get; set; }

        [DataMember(Name = "numberseries", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numberseries")]
        public List<NumberSeries> NumberSeries { get; set; }

        [DataMember(Name = "validationblock", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "validationblock")]
        public List<ValidationBlock> ValidationBlock { get; set; }

        /// <summary>
        /// Note field for additional information
        /// </summary>
        [DataMember(Name = "note", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }

    public class ValidationBlock
    {
        /// <summary>
        /// Contract identification (contractid or iban) that is requested to be terminated
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Value of the contract identification (contractid or iban) that is requested to be terminated 
        /// </summary>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    public class AddressBlock
    {
        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }

        [DataMember(Name = "housenr", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "housenr")]
        public string Housenr { get; set; }

        [DataMember(Name = "housenr_ext", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "housenr_ext")]
        public string HousenrExt { get; set; }
    }
}