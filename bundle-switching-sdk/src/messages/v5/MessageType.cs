using System.Collections.Generic;

namespace Coin.Sdk.BS.Messages.V5
{
    public enum MessageType
    {
        CancelV5,
        ContractTerminationRequestV5,
        ContractTerminationRequestAnswerV5,
        ContractTerminationPerformedV5,
        ErrorFoundV5
    }

    internal static class MessageTypeExtensions
    {
        private static readonly Dictionary<MessageType, string> MessageTypeNames = new Dictionary<MessageType, string>
        {
            [MessageType.CancelV5] = "cancel",
            [MessageType.ContractTerminationRequestV5] = "contractterminationrequest",
            [MessageType.ContractTerminationRequestAnswerV5] = "contractterminationrequestanswer",
            [MessageType.ContractTerminationPerformedV5] = "contractterminationperformed",
            [MessageType.ErrorFoundV5] = "errorfound"
        };

        internal static string Name(this MessageType messageType) => MessageTypeNames[messageType];
    }
}