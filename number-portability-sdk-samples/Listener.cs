using System;
using Coin.NP.Messages.V1;
using static Coin.NP.Messages.V1.Utils;
using Coin.NP.Service;
using Newtonsoft.Json;

namespace Tests
{
    public class Listener : INumberPortabilityMessageListener
    {
        public void OnMessage(string messageId, INpMessage<INpMessageContent> message) =>
            Console.WriteLine($"{TypeName(message)} {messageId}:\n{JsonConvert.SerializeObject(message, Formatting.Indented)}\n");
    }
}