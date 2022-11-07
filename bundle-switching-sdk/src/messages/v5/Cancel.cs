using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V5
{
    public class CancelMessage : IBsMessage<Cancel>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<CancelBody>))]
        public IBsMessageBody<Cancel> Body { get; set; }
    }

    public class CancelBody : IBsMessageBody<Cancel>
    {
        [DataMember(Name = "cancel", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cancel")]
        public Cancel Content { get; set; }
    }

    public class Cancel : IBsMessageContent
    {
        /// <summary>
        /// The identifier for the Contract Termination Request determined by the Recipient for communication between the Recipient and Donor. Defined as [RecipientSPCode]-[DonorSPCode]-[Identifier] The requirement is that the dossierid is unique and identifies the contract termination request. 
        /// </summary>
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        /// <summary>
        /// Note field for additional information
        /// </summary>
        [DataMember(Name = "note", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }
}