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
            _messageConsumer = new NumberPortabilityMessageConsumer(Consumer, PrivateKeyFile, EncryptedHmacSecretFile, _listener, SseUrl, backOffPeriod);
            _listener.Clear();
        }

        [Test]
        public void ConsumeAll()
        {
            _messageConsumer.StartConsuming();
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(100);
            }
            _messageConsumer.StopConsuming();
        }

        [Test]
        public void ConsumeFiltered()
        {
            _messageConsumer.StartConsuming(ConfirmationStatus.All, 3, new TestOffsetPersister(), v => v - 2, TypeName<PortingRequest>(), TypeName<PortingPerformed>());
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(100);
            }
            _messageConsumer.StopConsuming();
        }
    }
}