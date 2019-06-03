using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Coin.NP.Messages.V1 {

    public class ConfirmationMessage {

        [DataMember(Name="transactionId", EmitDefaultValue=false)]
        [JsonProperty(PropertyName = "transactionId")]
        public string TransactionId;
    }

    public enum ConfirmationStatus {
        Unconfirmed,
        All
    }
}
