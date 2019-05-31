using System;
using System.Collections.Generic;
using System.Threading;
using Coin.NP.Messages.V1;
using Coin.NP.Service.Impl;
using NUnit.Framework;
using static Tests.TestUtils;

namespace Tests
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
            const int backOffPeriod = 1;
            var listener = new Listener();
            _numberPortabilityService = new NumberPortabilityService(apiUrl, consumer, privateKeyFile, encryptedHmacSecretFile);
            _messageConsumer = new NumberPortabilityMessageConsumer(listener, sseUrl, consumer, backOffPeriod, privateKeyFile, encryptedHmacSecretFile);
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
                            //Recipientserviceprovider = "",
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
            var task = _numberPortabilityService.SendMessage(message);
            Console.WriteLine($"Transaction id: {task.Result.TransactionId}");
        }

        [Test]
        public void ConsumeMessages()
        {
            _messageConsumer.StartConsuming();
            Thread.Sleep(10000);
        }
    }
}