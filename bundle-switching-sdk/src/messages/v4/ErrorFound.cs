using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V4
{
    public class ErrorFoundMessage : IBsMessage<ErrorFound>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<ErrorFoundBody>))]
        public IBsMessageBody<ErrorFound> Body { get; set; }
    }

    public class ErrorFoundBody : IBsMessageBody<ErrorFound>
    {
        [DataMember(Name = "errorfound", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "errorfound")]
        public ErrorFound Content { get; set; }
    }

    public class ErrorFound : IBsMessageContent
    {
        /// <summary>
        /// The identifier for the Contract Termination Request determined by the Recipient for communication between the Recipient and Donor. Defined as [RecipientSPCode]-[DonorSPCode]-[Identifier] The requirement is that the dossierid is unique and identifies the contract termination request. 
        /// </summary>
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        /// <summary>
        /// The message type of the message in which one or more errors have been found.
        /// </summary>
        [DataMember(Name = "messagetype", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "messagetype")]
        public string MessageType { get; set; }

        /// <summary>
        /// The unique technical id of the message in which one or more errors have been found
        /// </summary>
        [DataMember(Name = "messageid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "messageid")]
        public string MessageId { get; set; }

        /// <summary>
        /// The processed message in which one or more errors have been found.
        /// </summary>
        [DataMember(Name = "originalmessage", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "originalmessage")]
        public string OriginalMessage { get; set; }

        /// <summary>
        /// A log of errors that has been found when processing the message.
        /// </summary>
        [DataMember(Name = "errorlog", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "errorlog")]
        public string ErrorLog { get; set; }

        [DataMember(Name = "repeats", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "repeats")]
        public List<ErrorFoundRepeats> Repeats { get; set; }
    }

    public class ErrorFoundRepeats
    {
        [DataMember(Name = "seq", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "seq")]
        public ErrorFoundSeq Seq { get; set; }
    }

    public class ErrorFoundSeq
    {
        /// <summary>
        /// [reserved for future use]
        /// </summary>
        [DataMember(Name = "phonenumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "phonenumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Id for the error situation 
        /// </summary>
        [DataMember(Name = "errorcode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "errorcode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Description of the errorcode 
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}