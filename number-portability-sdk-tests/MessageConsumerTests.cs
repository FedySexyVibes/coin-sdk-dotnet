using System;
using System.Diagnostics;
using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service.Impl;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using static Coin.Sdk.NP.Tests.TestSettings;

namespace Coin.Sdk.NP.Tests
{
    public class MessageConsumerTests
    {
        private NumberPortabilityMessageConsumer _messageConsumer;
        private StopStreamService _stopStreamService;
        private TestListener _listener;

        [SetUp]
        public void Setup()
        {
            var logger = NullLogger.Instance;

            var sseConsumer = new SseConsumer(logger, Consumer, SseUrl, PrivateKeyFile, EncryptedHmacSecretFile, numberOfRetries: NumberOfRetries);
            _stopStreamService = new StopStreamService(ApiUrl, Consumer, PrivateKeyFile, EncryptedHmacSecretFile);
            _messageConsumer = new NumberPortabilityMessageConsumer(sseConsumer, logger);
            _listener = new TestListener();
        }

        [Test]
        public void ConsumeUnconfirmed()
        {
            _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected"));
            Thread.Sleep(5000);
            _messageConsumer.StopConsuming();
        }

        [Test]
        public void ConsumeUnconfirmedWithInterrupt()
        {
            var numberMessages = 0;
            _listener.SideEffect = messageId =>
            {
                numberMessages++;
                if (messageId == "5") _stopStreamService.StopStream();
            };
            _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected"));
            Thread.Sleep(10_000);
            _messageConsumer.StopConsuming();
            Assert.Greater(numberMessages, 10);
        }

        [Test]
        public void ConsumeAllFiltered()
        {
            _messageConsumer.StartConsumingAll(_listener, new TestOffsetPersister(), 3,
                e => Assert.Fail("Disconnected"), MessageType.PortingRequestV1, MessageType.PortingPerformedV1);
            Thread.Sleep(5000);
            _messageConsumer.StopConsuming();
        }

        [Test]
        public void StopAndResumeConsuming()
        {
            var numberMessages = 0;
            _listener.SideEffect = messageId => numberMessages++;
            _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected"));
            Thread.Sleep(5000);
            _messageConsumer.StopConsuming();
            var numberMessagesAfterStop = numberMessages;
            Assert.Greater(numberMessagesAfterStop, 5);
            Thread.Sleep(3000);
            Assert.AreEqual(numberMessages, numberMessagesAfterStop);
            _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected"));
            Thread.Sleep(3000);
            Assert.Greater(numberMessages, numberMessagesAfterStop);
            _messageConsumer.StopConsuming();
        }

        [Test]
        public void CannotConsumeTwice()
        {
            _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected"));
            Assert.Throws(typeof(SynchronizationLockException),
                () => _messageConsumer.StartConsumingUnconfirmed(_listener, e => Assert.Fail("Disconnected")));
            _messageConsumer.StopConsuming();
        }
    }
}