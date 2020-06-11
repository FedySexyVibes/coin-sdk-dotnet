using System;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service;

namespace Coin.Sdk.NP.Sample
{
    public class Listener : INumberPortabilityMessageListener
    {
        public void OnActivationServiceNumber(string messageId, ActivationServiceNumberMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnCancel(string messageId, CancelMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnDeactivation(string messageId, DeactivationMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnDeactivationServiceNumber(string messageId, DeactivationServiceNumberMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumActivationNumber(string messageId, EnumActivationNumberMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumActivationOperator(string messageId, EnumActivationOperatorMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumActivationRange(string messageId, EnumActivationRangeMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumDeactivationNumber(string messageId, EnumDeactivationNumberMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumDeactivationOperator(string messageId, EnumDeactivationOperatorMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumDeactivationRange(string messageId, EnumDeactivationRangeMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumProfileActivation(string messageId, EnumProfileActivationMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnEnumProfileDeactivation(string messageId, EnumProfileDeactivationMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnErrorFound(string messageId, ErrorFoundMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnException(Exception exception)
        {
            System.Diagnostics.Debug.WriteLine($"Exception received {exception}");
        }

        public void OnKeepAlive()
        {
            System.Diagnostics.Debug.WriteLine("Keepalive");
        }

        public void OnPortingPerformed(string messageId, PortingPerformedMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnPortingRequest(string messageId, PortingRequestMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnPortingRequestAnswer(string messageId, PortingRequestAnswerMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnPortingRequestAnswerDelayed(string messageId, PortingRequestAnswerDelayedMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnRangeActivation(string messageId, RangeActivationMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnRangeDeactivation(string messageId, RangeDeactivationMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnTariffChangeServiceNumber(string messageId, TariffChangeServiceNumberMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnUnknownMessage(string messageId, string message)
        {
            System.Diagnostics.Debug.WriteLine($"Unknown message received: {message}");
        }
    }
}