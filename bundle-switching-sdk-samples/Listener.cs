using System;
using Coin.Sdk.BS.Messages.V4;
using Coin.Sdk.BS.Service;

namespace Coin.Sdk.BS.Sample
{
    public class Listener : IBundleSwitchingMessageListener
    {
        public void OnCancel(string messageId, CancelMessage message)
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

        public void OnContractTerminationPerformed(string messageId, ContractTerminationPerformedMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnContractTerminationRequest(string messageId, ContractTerminationRequestMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnContractTerminationRequestAnswer(string messageId, ContractTerminationRequestAnswerMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"Received message with id {messageId} of type {message.GetType()}");
        }

        public void OnUnknownMessage(string messageId, string message)
        {
            System.Diagnostics.Debug.WriteLine($"Unknown message received: {message}");
        }
    }
}