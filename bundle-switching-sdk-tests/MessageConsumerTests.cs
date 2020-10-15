using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.BS.Messages.V4;
using Coin.Sdk.BS.Service.Impl;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using static Coin.Sdk.BS.Tests.TestSettings;

namespace Coin.Sdk.BS.Tests
{
    public class MessageConsumerTests
    {
        private BundleSwitchingMessageConsumer _messageConsumer;
        private TestListener _listener;

        [SetUp]
        public void Setup()
        {
            var logger = NullLogger.Instance;

            var sseConsumer = new SseConsumer(logger, Consumer, SseUrl, PrivateKeyFile, EncryptedHmacSecretFile);
            _messageConsumer = new BundleSwitchingMessageConsumer(sseConsumer, logger);
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
                e => Assert.Fail("Disconnected"), null, MessageType.ContractTerminationRequestV4, MessageType.ContractTerminationPerformedV4);
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }

            _messageConsumer.StopConsuming();
        }
    }
}