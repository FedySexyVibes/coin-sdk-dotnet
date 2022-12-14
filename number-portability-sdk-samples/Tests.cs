using System;
using System.Collections.Generic;
using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.NP.Messages.V3;
using Coin.Sdk.NP.Service.Impl;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using static Coin.Sdk.NP.Sample.TestUtils;

namespace Coin.Sdk.NP.Sample
{
    public class Tests
    {
        private NumberPortabilityService _numberPortabilityService = null!;
        private NumberPortabilityMessageConsumer _messageConsumer = null!;

        private const string Operator = "<YOUR OPERATOR>";
        private const string ReceivingOperator = "<RECEIVING OPERATOR>";
        private readonly string _timestamp = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
        private const string PhoneNumber = "0303800007";

        [SetUp]
        public void Setup()
        {
            var logger = NullLogger.Instance;

            const string apiUrl = "https://test-api.coin.nl/number-portability/v3";
            const string sseUrl = apiUrl + "/dossiers/events"; 
            const string consumer = "<YOUR CONSUMER>";
            var privateKeyFile = GetPath("private-key.pem");
            var encryptedHmacSecretFile =  GetPath("sharedkey.encrypted");
            _numberPortabilityService = new NumberPortabilityService(apiUrl, consumer, privateKeyFile, encryptedHmacSecretFile);
            var sseConsumer = new SseConsumer(logger, consumer, sseUrl, privateKeyFile, encryptedHmacSecretFile, 1, 0);
            _messageConsumer = new NumberPortabilityMessageConsumer(sseConsumer, logger);
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
                                new()
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
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (response is not ErrorResponse error) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void SendPortingRequestAnswer()
        {
            var dossierId = "<dossier-id>"; // dossier id of an existing porting request should be used here
            Console.WriteLine($"Sending porting request answer with dossier id {dossierId}");

            var message = new MessageEnvelope<PortingRequestAnswer>
            {
                Message = new PortingRequestAnswerMessage()
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
                            NetworkOperator = ReceivingOperator
                            //ServiceProvider = ""
                        },
                        Timestamp = _timestamp
                    },
                    Body = new PortingRequestAnswerBody
                    {
                        Content = new PortingRequestAnswer
                        {
                            DossierId = dossierId,
                            Blocking = "N",
                            Repeats = new List<PortingRequestAnswerRepeats>
                            {
                                new()
                                {
                                    Seq = new PortingRequestAnswerSeq
                                    {
                                        DonorNetworkOperator = Operator,
                                        DonorServiceProvider = Operator,
                                        FirstPossibleDate = DateTime.UtcNow.ToString("yyyyMMddhhmmss"),
                                        NumberSeries = new NumberSeries
                                        {
                                            Start = PhoneNumber,
                                            End = PhoneNumber
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (response is not ErrorResponse error) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void SendPortingRequestAnswerDelayed()
        {
            var dossierId = "<dossier-id>"; // dossier id of an existing porting request should be used here
            Console.WriteLine($"Sending porting request answer with dossier id {dossierId}");

            var message = new MessageEnvelope<PortingRequestAnswerDelayed>
            {
                Message = new PortingRequestAnswerDelayedMessage()
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
                            NetworkOperator = ReceivingOperator
                            //ServiceProvider = ""
                        },
                        Timestamp = _timestamp
                    },
                    Body = new PortingRequestAnswerDelayedBody
                    {
                        Content = new PortingRequestAnswerDelayed
                        {
                            DossierId = dossierId,
                            DonorNetworkOperator = Operator,
                            ReasonCode = "99"
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (response is not ErrorResponse error) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void SendPortingPerformed()
        {
            var dossierId = "<dossier-id>"; // dossier id of an existing porting request should be used here
            Console.WriteLine($"Sending porting request answer with dossier id {dossierId}");

            var message = new MessageEnvelope<PortingPerformed>
            {
                Message = new PortingPerformedMessage()
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
                            NetworkOperator = ReceivingOperator
                            //ServiceProvider = ""
                        },
                        Timestamp = _timestamp
                    },
                    Body = new PortingPerformedBody
                    {
                        Content = new PortingPerformed
                        {
                            DossierId = dossierId,
                            DonorNetworkOperator = Operator,
                            RecipientNetworkOperator = Operator,
                            
                            Repeats = new List<PortingPerformedRepeats>
                            {
                                new()
                                {
                                    Seq = new PortingPerformedSeq
                                    {
                                        NumberSeries = new NumberSeries
                                        {
                                            Start = PhoneNumber,
                                            End = PhoneNumber
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (response is not ErrorResponse error) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void SendCancel()
        {
            var dossierId = "<dossier-id>"; // dossier id of an existing porting request should be used here
            Console.WriteLine($"Sending porting request answer with dossier id {dossierId}");

            var message = new MessageEnvelope<Cancel>
            {
                Message = new CancelMessage()
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
                            NetworkOperator = ReceivingOperator
                            //ServiceProvider = ""
                        },
                        Timestamp = _timestamp
                    },
                    Body = new CancelBody
                    {
                        Content = new Cancel
                        {
                            DossierId = dossierId,
                            Note = "some additional information"
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (response is not ErrorResponse error) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void SendDeactivation()
        {
            var dossierId = "<dossier-id>"; // dossier id of an existing porting request should be used here
            Console.WriteLine($"Sending porting request answer with dossier id {dossierId}");

            var message = new MessageEnvelope<Deactivation>
            {
                Message = new DeactivationMessage()
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
                            NetworkOperator = AllOperators
                            //ServiceProvider = ""
                        },
                        Timestamp = _timestamp
                    },
                    Body = new DeactivationBody
                    {
                        Content = new Deactivation
                        {
                            DossierId = dossierId,
                            OriginalNetworkOperator = ReceivingOperator,
                            CurrentNetworkOperator = Operator,
                            Repeats = new List<DeactivationRepeats>
                            {
                                new()
                                {
                                    Seq = new DeactivationSeq
                                    {
                                        NumberSeries = new NumberSeries
                                        {
                                            Start = PhoneNumber,
                                            End = PhoneNumber
                                        },
                                    }
                                }
                            }
                        }
                    }
                }
            };
            var response = _numberPortabilityService.SendMessageAsync(message).Result;
            Console.WriteLine($"Transaction id: {response.TransactionId}");
            if (response is not ErrorResponse error) return;
            foreach (var content in error.Errors)
            {
                Console.WriteLine($"Error {content.Code}: {content.Message}");
            }
            Assert.Fail();
        }

        [Test]
        public void ConsumeMessages()
        {
            _messageConsumer.StartConsumingUnconfirmed(new Listener(), _ => Assert.Fail("Disconnected"));
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