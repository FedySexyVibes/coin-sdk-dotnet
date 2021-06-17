using System.Collections.Generic;

namespace Coin.Sdk.NP.Messages.V3
{
    public enum MessageType
    {
        ActivationServiceNumberV3,
        CancelV3,
        DeactivationV3,
        DeactivationServiceNumberV3,
        EnumActivationNumberV3,
        EnumActivationOperatorV3,
        EnumActivationRangeV3,
        EnumDeactivationNumberV3,
        EnumDeactivationOperatorV3,
        EnumDeactivationRangeV3,
        EnumProfileActivationV3,
        EnumProfileDeactivationV3,
        ErrorFoundV3,
        PortingRequestV3,
        PortingRequestAnswerV3,
        PortingPerformedV3,
        PortingRequestAnswerDelayedV3,
        RangeActivationV3,
        RangeDeactivationV3,
        TariffChangeServiceNumberV3
    }

    internal static class MessageTypeExtensions
    {
        private static readonly Dictionary<MessageType, string> MessageTypeNames = new Dictionary<MessageType, string>
        {
            [MessageType.ActivationServiceNumberV3] = "activationsn",
            [MessageType.CancelV3] = "cancel",
            [MessageType.DeactivationV3] = "deactivation",
            [MessageType.DeactivationServiceNumberV3] = "deactivationsn",
            [MessageType.EnumActivationNumberV3] = "enumactivationnumber",
            [MessageType.EnumActivationOperatorV3] = "enumactivationoperator",
            [MessageType.EnumActivationRangeV3] = "enumactivationrange",
            [MessageType.EnumDeactivationNumberV3] = "enumdeactivationnumber",
            [MessageType.EnumDeactivationOperatorV3] = "enumdeactivationoperator",
            [MessageType.EnumDeactivationRangeV3] = "enumdeactivationrange",
            [MessageType.EnumProfileActivationV3] = "enumprofileactivation",
            [MessageType.EnumProfileDeactivationV3] = "enumprofiledeactivation",
            [MessageType.ErrorFoundV3] = "errorfound",
            [MessageType.PortingRequestV3] = "portingrequest",
            [MessageType.PortingRequestAnswerV3] = "portingrequestanswer",
            [MessageType.PortingPerformedV3] = "portingperformed",
            [MessageType.PortingRequestAnswerDelayedV3] = "pradelayed",
            [MessageType.RangeActivationV3] = "rangeactivation",
            [MessageType.RangeDeactivationV3] = "rangedeactivation",
            [MessageType.TariffChangeServiceNumberV3] = "tariffchangesn"
        };

        internal static string Name(this MessageType messageType) => MessageTypeNames[messageType];
    }
}