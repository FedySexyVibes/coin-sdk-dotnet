using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.NP.Tests.TestSettings;

namespace Coin.Sdk.NP.Tests
{
    public class MessageConsumerTests
    {
        private NumberPortabilityMessageConsumer _messageConsumer;
        private TestListener _listener;

        [SetUp]
        public void Setup()
        {
            var sseConsumer = new SseConsumer(Consumer, SseUrl, PrivateKeyFile, EncryptedHmacSecretFile);
            _messageConsumer = new NumberPortabilityMessageConsumer(sseConsumer);
            _listener = new TestListener();
        }

        [Test]
        public void ConsumeUnconfirmed()
        {
            _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected"));
            for (var i = 0; i < 30; i++)
            {
                Thread.Sleep(1000);
            }

            _messageConsumer.StopConsuming();
        }

        [Test]
        public void ConsumeAllFiltered()
        {
            _messageConsumer.StartConsumingAll(_listener, new TestOffsetPersister(), 3,
                e => Assert.Fail("Disconnected"), MessageType.PortingRequestV1, MessageType.PortingPerformedV1);
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }

            _messageConsumer.StopConsuming();
        }
    }
}