using System;
using System.Collections.Generic;
using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V1;
using Coin.Sdk.NP.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.NP.Sample.TestUtils;

namespace Coin.Sdk.NP.Sample
{
    public class Tests
    {
        NumberPortabilityService _numberPortabilityService;
        NumberPortabilityMessageConsumer _messageConsumer;
        
        const string Operator = "<YOUR OPERATOR>";
        readonly string _timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
        const string PhoneNumber = "0612345678";

        [SetUp]
        public void Setup()
        {
            const string apiUrl = "https://test-api.coin.nl/number-portability/v1";
            const string sseUrl = apiUrl + "/dossiers/events"; 
            const string consumer = "<YOUR CONSUMER>";
            var privateKeyFile = GetPath("private-key.pem");
            var encryptedHmacSecretFile =  GetPath("sharedkey.encrypted");
            _numberPortabilityService = new NumberPortabilityService(apiUrl, consumer, privateKeyFile, encryptedHmacSecretFile);
            var sseConsumer = new SseConsumer(consumer, sseUrl, privateKeyFile, encryptedHmacSecretFile, 1, 0);
            _messageConsumer = new NumberPortabilityMessageConsumer(sseConsumer);
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
                            ServiceProvider = Operator
                        },
                        Receiver = new Receiver
                        {
                            NetworkOperator = CrdbReceiver
                            //ServiceProvider = ""
                        },
                        Timestamp = _timestamp
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
                                        }
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
                            }
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
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (!(response is ErrorResponse error)) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void ConsumeMessages()
        {
            _messageConsumer.StartConsumingUnconfirmed(new Listener(), e => Assert.Fail("Disconnected"));
            Thread.Sleep(1000);
        }
        
        [Test]
        public void SendConfirmation()
        {
            const string messageId = "<ENTER MESSAGE ID>";
            var response = _numberPortabilityService.SendConfirmationAsync(messageId).Result;
            Console.WriteLine(response.IsSuccessStatusCode
                ? $"Successfully sent confirmation of message {messageId}"
                : $"Confirmation failed with status {response.StatusCode}. Response: {response.Content.ReadAsStringAsync().Result}.");

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}