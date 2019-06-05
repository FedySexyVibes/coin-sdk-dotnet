using System;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Sample
{
    public class Listener : INumberPortabilityMessageListener
    {
        public void OnMessage(string messageId, INpMessage<INpMessageContent> message) =>
            Console.WriteLine($"{Utils.TypeName(message)} {messageId}:\n{JsonConvert.SerializeObject(message, Formatting.Indented)}\n");
    }
}