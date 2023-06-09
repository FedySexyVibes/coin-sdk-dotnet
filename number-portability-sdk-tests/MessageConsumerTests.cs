﻿using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V3;
using Coin.Sdk.NP.Service.Impl;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using static Coin.Sdk.NP.Tests.TestSettings;

namespace Coin.Sdk.NP.Tests
{
    public class MessageConsumerTests
    {
        private NumberPortabilityMessageConsumer _messageConsumer = null!;
        private StopStreamService _stopStreamService = null!;
        private TestListener _listener = null!;

        [SetUp]
        public void Setup()
        {
            var logger = NullLogger.Instance;

            var sseConsumer = new SseConsumer(logger, Consumer, SseUrl, PrivateKeyFile, EncryptedHmacSecretFile, numberOfRetries: NumberOfRetries);
            _stopStreamService = new StopStreamService(ApiUrl, Consumer, PrivateKeyFile, EncryptedHmacSecretFile);
            _messageConsumer = new NumberPortabilityMessageConsumer(sseConsumer, logger);
            _listener = new TestListener();
        }

        [Test, Timeout(5000)]
        public void ConsumeUnconfirmed()
        {
            var cde = new CountdownEvent(5);
            _listener.SideEffect = _ => cde.Signal();
            _messageConsumer.StartConsumingUnconfirmed(_listener, _ => Assert.Fail("Disconnected"));
            cde.Wait();
            _messageConsumer.StopConsuming();
        }

        [Test, Timeout(8000)]
        public void ConsumeUnconfirmedWithInterrupt()
        {
            var cde = new CountdownEvent(5);
            _listener.SideEffect = _ => cde.Signal();
            _messageConsumer.StartConsumingUnconfirmed(_listener, _ => Assert.Fail("Disconnected"));
            cde.Wait();
            _stopStreamService.StopStream();
            cde.Reset();
            cde.Wait();
            _messageConsumer.StopConsuming();
        }

        [Test, Timeout(5000)]
        public void ConsumeAllFiltered()
        {
            var cde = new CountdownEvent(3);
            _listener.SideEffect = _ => cde.Signal();
            _messageConsumer.StartConsumingAll(_listener, new TestOffsetPersister(), 3,
                e => Assert.Fail("Disconnected"), MessageType.PortingRequestV3, MessageType.PortingPerformedV3);
            cde.Wait();
            _messageConsumer.StopConsuming();
        }

        [Test, Timeout(15000)]
        public void StopAndResumeConsuming()
        {
            var cde = new CountdownEvent(5);
            _listener.SideEffect = _ => cde.Signal();
            _messageConsumer.StartConsumingUnconfirmed(_listener, _ => Assert.Fail("Disconnected"));
            cde.Wait();
            _messageConsumer.StopConsuming();
            cde.Reset();
            Thread.Sleep(3000);
            Assert.AreEqual(cde.CurrentCount, cde.InitialCount);
            _messageConsumer.StartConsumingUnconfirmed(_listener, _ => Assert.Fail("Disconnected"));
            cde.Wait();
            _messageConsumer.StopConsuming();
        }

        [Test, Timeout(5000)]
        public void DoesNotConsumeTwice()
        {
            var firstListener = new TestListener();
            var cde = new CountdownEvent(5);
            _listener.SideEffect = _ => cde.Signal();
            _messageConsumer.StartConsumingUnconfirmed(firstListener, _ => Assert.Fail("Disconnected"));
            _messageConsumer.StartConsumingUnconfirmed(_listener, _ => Assert.Fail("Disconnected"));
            firstListener.SideEffect = _ => Assert.Fail("First event stream has not been closed");
            cde.Wait();
            _messageConsumer.StopConsuming();
            
        }
    }
}