using System.Collections.Generic;
using System.Threading;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.NP.Tests.TestSettings;
using static Coin.Sdk.NP.Tests.TestUtils;

namespace Coin.Sdk.NP.Tests
{
    public class NumberPortabilityServiceTests
    {
        NumberPortabilityService _numberPortabilityService;

        [SetUp]
        public void Setup()
        {
            _numberPortabilityService = new NumberPortabilityService(ApiUrl, Consumer, PrivateKeyFile, EncryptedHmacSecretFile);
        }

        MessageResponse SendPortingRequest(string dossierId)
        {
            var message = new MessageEnvelope<PortingRequest>
            {
                Message = new PortingRequestMessage
                {
                    Header = new Header
                    {
                        Sender = new Sender
                        {
                            NetworkOperator = Operator
                        },
                        Receiver = new Receiver
                        {
                            NetworkOperator = CrdbReceiver
                        },
                        Timestamp = Timestamp
                    },
                    Body = new PortingRequestBody
                    {
                        Content = new PortingRequest
                        {
                            DossierId = dossierId,
                            RecipientNetworkOperator = Operator,
                            Repeats = new List<PortingRequestRepeats>
                            {
                                new PortingRequestRepeats
                                {
                                    Seq = new PortingRequestSeq
                                    {
                                        NumberSeries = new NumberSeries
                                        {
                                            Start = PhoneNumber,
                                            End = PhoneNumber
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return _numberPortabilityService.SendMessage(message).Result;
        }

        [Test]
        public void SendPortingRequest()
        {
            var dossierId = GenerateDossierId(Operator);
            var response = SendPortingRequest(dossierId);
            Assert.IsNotInstanceOf<ErrorResponse>(response);
        }

        [Test]
        public void SendInvalidPortingRequest()
        {
            var dossierId = GenerateDossierId(Operator);
            var response = SendPortingRequest(dossierId + "invalid");
            Assert.IsInstanceOf<ErrorResponse>(response);
        }
    }
}