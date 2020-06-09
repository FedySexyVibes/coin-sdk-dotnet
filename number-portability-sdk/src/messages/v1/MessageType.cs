using System.Collections.Generic;

namespace Coin.Sdk.NP.Messages.V1
{
    public enum MessageType
    {
        ActivationServiceNumberV1,
        CancelV1,
        DeactivationV1,
        DeactivationServiceNumberV1,
        EnumActivationNumberV1,
        EnumActivationOperatorV1,
        EnumActivationRangeV1,
        EnumDeactivationNumberV1,
        EnumDeactivationOperatorV1,
        EnumDeactivationRangeV1,
        EnumProfileActivationV1,
        EnumProfileDeactivationV1,
        ErrorFoundV1,
        PortingRequestV1,
        PortingRequestAnswerV1,
        PortingPerformedV1,
        PortingRequestAnswerDelayedV1,
        RangeActivationV1,
        RangeDeactivationV1,
        TariffChangeServiceNumberV1
    }

    internal static class MessageTypeExtensions
    {
        private static readonly Dictionary<MessageType, string> MessageTypeNames = new Dictionary<MessageType, string>
        {
            [MessageType.ActivationServiceNumberV1] = "activationsn",
            [MessageType.CancelV1] = "cancel",
            [MessageType.DeactivationV1] = "deactivation",
            [MessageType.DeactivationServiceNumberV1] = "deactivationsn",
            [MessageType.EnumActivationNumberV1] = "enumactivationnumber",
            [MessageType.EnumActivationOperatorV1] = "enumactivationoperator",
            [MessageType.EnumActivationRangeV1] = "enumactivationrange",
            [MessageType.EnumDeactivationNumberV1] = "enumdeactivationnumber",
            [MessageType.EnumDeactivationOperatorV1] = "enumdeactivationoperator",
            [MessageType.EnumDeactivationRangeV1] = "enumdeactivationrange",
            [MessageType.EnumProfileActivationV1] = "enumprofileactivation",
            [MessageType.EnumProfileDeactivationV1] = "enumprofiledeactivation",
            [MessageType.ErrorFoundV1] = "errorfound",
            [MessageType.PortingRequestV1] = "portingrequest",
            [MessageType.PortingRequestAnswerV1] = "portingrequestanswer",
            [MessageType.PortingPerformedV1] = "portingperformed",
            [MessageType.PortingRequestAnswerDelayedV1] = "pradelayed",
            [MessageType.RangeActivationV1] = "rangeactivation",
            [MessageType.RangeDeactivationV1] = "rangedeactivation",
            [MessageType.TariffChangeServiceNumberV1] = "tariffchangesn"
        };

        internal static string Name(this MessageType messageType) => MessageTypeNames[messageType];
    }
}