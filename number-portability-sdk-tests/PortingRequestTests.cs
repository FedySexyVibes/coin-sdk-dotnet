using System;
using System.Collections.Generic;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.NP.Tests.TestSettings;
using static Coin.Sdk.NP.Tests.TestUtils;

namespace Coin.Sdk.NP.Tests
{
    public class PortingRequestTests
    {
        NumberPortabilityService _numberPortabilityService;
        NumberPortabilityMessageConsumer _messageConsumer;

        [SetUp]
        public void Setup()
        {
            const int backOffPeriod = 1;
            var listener = new TestListener();
            _numberPortabilityService = new NumberPortabilityService(ApiUrl, Consumer, PrivateKeyFile, EncryptedHmacSecretFile);
            _messageConsumer = new NumberPortabilityMessageConsumer(Consumer, PrivateKeyFile, EncryptedHmacSecretFile, listener, SseUrl, backOffPeriod);
        }

        [Test]
        public void SendPortingRequest()
        {
            var dossierId = GenerateDossierId(Operator);
            Console.WriteLine($"Sending porting request with dossier id {dossierId}");

            var message = new MessageEnvelope<PortingRequest>
            {
                Message = new PortingRequestMessage
                {
                    Header = new Header
                    {
                        Sender = new Sender
                        {
                            NetworkOperator = Operator,
                            //ServiceProvider = ""
                        },
                        Receiver = new Receiver
                        {
                            NetworkOperator = CrdbReceiver,
                            //ServiceProvider = ""
                        },
                        Timestamp = Timestamp
                    },
                    Body = new PortingRequestBody
                    {
                        Content = new PortingRequest
                        {
                            DossierId = dossierId,
                            //DonorNetworkOperator = "",
                            //DonorServiceProvider = "",
                            RecipientNetworkOperator = Operator,
                            //RecipientServiceProvider = "",
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
                                        },
                                        /*Repeats = new List<EnumRepeats>
                                        {
                                            new EnumRepeats
                                            {
                                                Seq = new EnumProfileSeq
                                                {
                                                    ProfileId = ""
                                                }
                                            }
                                        }*/
                                    }
                                }
                            },
                            /*CustomerInfo = new CustomerInfo
                            {
                                //CustomerId = "",
                                //Companyname = "",
                                //Lastname = "",
                                //Postcode = "",
                                //HouseNr = "",
                                //HouseNrExt = ""
                            },
                            Note = ""*/
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessage(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (!(response is ErrorResponse error)) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }
    }
}