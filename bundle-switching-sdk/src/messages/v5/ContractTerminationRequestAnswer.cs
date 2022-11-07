using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V5
{
    public class ContractTerminationRequestAnswerMessage : IBsMessage<ContractTerminationRequestAnswer>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<ContractTerminationRequestAnswerBody>))]
        public IBsMessageBody<ContractTerminationRequestAnswer> Body { get; set; }
    }

    public class ContractTerminationRequestAnswerBody : IBsMessageBody<ContractTerminationRequestAnswer>
    {
        [DataMember(Name = "contractterminationrequestanswer", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "contractterminationrequestanswer")]
        public ContractTerminationRequestAnswer Content { get; set; }
    }

    public class ContractTerminationRequestAnswer : IBsMessageContent
    {
        /// <summary>
        /// The identifier for the Contract Termination Request determined by the Recipient for communication between the Recipient and Donor. Defined as [RecipientSPCode]-[DonorSPCode]-[Identifier] The requirement is that the dossierid is unique and identifies the contract termination request. 
        /// </summary>
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        /// <summary>
        /// Blocking indicator as a response to the contract termination request message. 
        /// </summary>
        [DataMember(Name = "blocking", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "blocking")]
        public string Blocking { get; set; }

        /// <summary>
        /// Blocking code as described in E2E Overstappen. If blockingcode = 0 then blocking=N else blocking=Y
        /// </summary>
        [DataMember(Name = "blockingcode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "blockingcode")]
        public string BlockingCode { get; set; }

        /// <summary>
        /// The first possible date that the contract termination can take place. Format: CCYY-MM-DDTHH24:MI:SS 
        /// </summary>
        [DataMember(Name = "firstpossibledate", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "firstpossibledate")]
        public string FirstPossibleDate { get; set; }

        [DataMember(Name = "infrablock", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "infrablock")]
        public InfraBlock InfraBlock { get; set; }

        /// <summary>
        /// Note field for additional information
        /// </summary>
        [DataMember(Name = "note", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }

    public class InfraBlock
    {
        /// <summary>
        /// Provider of the infra of the line
        /// </summary>
        [DataMember(Name = "infraprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "infraprovider")]
        public string InfraProvider { get; set; }

        /// <summary>
        /// Type of infra of the line 
        /// </summary>
        [DataMember(Name = "infratype", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "infratype")]
        public string InfraType { get; set; }

        /// <summary>
        /// Id of the infra of the line 
        /// </summary>
        [DataMember(Name = "infraid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "infraid")]
        public string InfraId { get; set; }
    }
}