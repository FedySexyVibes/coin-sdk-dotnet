using System;
using Coin.Sdk.NP.Messages.V1;
using static Coin.Sdk.NP.Messages.V1.Utils;
using Coin.Sdk.NP.Service;
using Newtonsoft.Json;

namespace Coin.Sdk.NP.Tests
{
    public class TestListener : INumberPortabilityMessageListener
    {
        public void OnMessage(string messageId, INpMessage<INpMessageContent> message) =>
            Console.WriteLine($"{TypeName(message)} {messageId}:\n{JsonConvert.SerializeObject(message, Formatting.Indented)}\n");
    }
}