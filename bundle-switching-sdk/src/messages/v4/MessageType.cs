using System.Collections.Generic;

namespace Coin.Sdk.BS.Messages.V4
{
    public enum MessageType
    {
        CancelV4,
        ContractTerminationRequestV4,
        ContractTerminationRequestAnswerV4,
        ContractTerminationPerformedV4,
        ErrorFoundV4
    }

    internal static class MessageTypeExtensions
    {
        private static readonly Dictionary<MessageType, string> MessageTypeNames = new Dictionary<MessageType, string>
        {
            [MessageType.CancelV4] = "cancel",
            [MessageType.ContractTerminationRequestV4] = "contractterminationrequest",
            [MessageType.ContractTerminationRequestAnswerV4] = "contractterminationrequestanswer",
            [MessageType.ContractTerminationPerformedV4] = "contractterminationperformed",
            [MessageType.ErrorFoundV4] = "errorfound"
        };

        internal static string Name(this MessageType messageType) => MessageTypeNames[messageType];
    }
}