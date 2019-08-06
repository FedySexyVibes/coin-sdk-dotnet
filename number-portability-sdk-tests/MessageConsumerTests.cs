using System.Threading;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.NP.Tests.TestSettings;
using static Coin.Sdk.NP.Messages.V1.Utils;

namespace Coin.Sdk.NP.Tests
{
    public class MessageConsumerTests
    {
        NumberPortabilityMessageConsumer _messageConsumer;
        TestListener _listener;

        [SetUp]
        public void Setup()
        {
            const int backOffPeriod = 1;
            _listener = new TestListener();
            _messageConsumer = new NumberPortabilityMessageConsumer(Consumer, PrivateKeyFile, EncryptedHmacSecretFile, _listener, SseUrl, backOffPeriod, 20);
            _listener.Clear();
        }

        [Test]
        public void ConsumeAll()
        {
            _messageConsumer.StartConsuming(onFinalDisconnect: e => Assert.Fail("Disconnected"));
            for (var i = 0; i < 300; i++)
            {
                Thread.Sleep(1000);
            }
            _messageConsumer.StopConsuming();
        }

        [Test]
        public void ConsumeFiltered()
        {
            _messageConsumer.StartConsuming(ConfirmationStatus.All, 3, new TestOffsetPersister(), v => v - 2, e => Assert.Fail("Disconnected"), TypeName<PortingRequest>(), TypeName<PortingPerformed>());
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
            }
            _messageConsumer.StopConsuming();
        }
    }
}