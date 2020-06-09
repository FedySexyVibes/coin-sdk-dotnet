using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V4
{
    public class ContractTerminationPerformedMessage : IBsMessage<ContractTerminationPerformed>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<ContractTerminationPerformedBody>))]
        public IBsMessageBody<ContractTerminationPerformed> Body { get; set; }
    }

    public class ContractTerminationPerformedBody : IBsMessageBody<ContractTerminationPerformed>
    {
        [DataMember(Name = "contractterminationperformed", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "contractterminationperformed")]
        public ContractTerminationPerformed Content { get; set; }
    }

    public class ContractTerminationPerformed : IBsMessageContent
    {
        /// <summary>
        /// The identifier for the Contract Termination Request determined by the Recipient for communication between the Recipient and Donor. Defined as [RecipientSPCode]-[DonorSPCode]-[Identifier] The requirement is that the dossierid is unique and identifies the contract termination request. 
        /// </summary>
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        /// <summary>
        /// The date and time on which the termination of the contract has  been performed. COMP will register the datetime of reception of the Contract Termination Performed message as the actualdatetime and populate this field in the message when sending to the Donor. Format: CCYY-MM-DDTHH24:MI:SS 
        /// </summary>
        [DataMember(Name = "actualdatetime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "actualdatetime")]
        public string ActualDatetime { get; set; }

        /// <summary>
        /// Note field for additional information
        /// </summary>
        [DataMember(Name = "note", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }
}