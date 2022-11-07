using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.Sdk.BS.Messages.V5
{
    public class ConfirmationMessage
    {
        [DataMember(Name = "transactionId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId { get; set; }
    }
}