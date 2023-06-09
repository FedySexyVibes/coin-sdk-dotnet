using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Messages.V3
{
    public class PortingRequestAnswerDelayedMessage : INpMessage<PortingRequestAnswerDelayed>
    {
        [DataMember(Name = "header", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "header")]
        public Header Header { get; set; }

        [DataMember(Name = "body", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "body")]
        [JsonConverter(typeof(ConcreteConverter<PortingRequestAnswerDelayedBody>))]
        public INpMessageBody<PortingRequestAnswerDelayed> Body { get; set; }
    }

    public class PortingRequestAnswerDelayedBody : INpMessageBody<PortingRequestAnswerDelayed>
    {
        [DataMember(Name = "pradelayed", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pradelayed")]
        public PortingRequestAnswerDelayed Content { get; set; }
    }

    public class PortingRequestAnswerDelayed : INpMessageContent
    {
        [DataMember(Name = "dossierid", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "dossierid")]
        public string DossierId { get; set; }

        [DataMember(Name = "donornetworkoperator", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "donornetworkoperator")]
        public string DonorNetworkOperator { get; set; }

        [DataMember(Name = "donorserviceprovider", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "donorserviceprovider")]
        public string DonorServiceProvider { get; set; }

        [DataMember(Name = "answerduedatetime", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "answerduedatetime")]
        public string AnswerDueDatetime { get; set; }

        [DataMember(Name = "reasoncode", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "reasoncode")]
        public string ReasonCode { get; set; }
    }
}