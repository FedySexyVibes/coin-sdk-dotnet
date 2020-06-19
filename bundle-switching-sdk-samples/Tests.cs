using System;
using System.Collections.Generic;
using System.Threading;
using Coin.Sdk.Common.Client;
using Coin.Sdk.BS.Messages.V4;
using Coin.Sdk.BS.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.BS.Sample.TestUtils;

namespace Coin.Sdk.BS.Sample
{
    public class Tests
    {
        private BundleSwitchingService _contractTerminationService;
        private BundleSwitchingMessageConsumer _messageConsumer;

        private const string Sender = "<YOUR PROVIDER>";
        private const string Receiver = "<DONOR PROVIDER>";
        private readonly string _timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        private const string PhoneNumber = "0612345678";

        [SetUp]
        public void Setup()
        {
            const string apiUrl = "https://test-api.coin.nl/bundle-switching/v4";
            const string sseUrl = apiUrl + "/dossiers/events"; 
            const string consumer = "<YOUR CONSUMER>";
            var privateKeyFile = GetPath("private-key.pem");
            var encryptedHmacSecretFile =  GetPath("sharedkey.encrypted");
            _contractTerminationService = new BundleSwitchingService(apiUrl, consumer, privateKeyFile, encryptedHmacSecretFile);
            var sseConsumer = new SseConsumer(consumer, sseUrl, privateKeyFile, encryptedHmacSecretFile, 1, 0);
            _messageConsumer = new BundleSwitchingMessageConsumer(sseConsumer);
        }

        [Test]
        public void SendContractTerminationRequest()
        {
            var dossierId = GenerateDossierId(Sender, Receiver);
            Console.WriteLine($"Sending contract termination request with dossier id {dossierId}");

            var message = new MessageEnvelope<ContractTerminationRequest>
            {
                Message = new ContractTerminationRequestMessage
                {
                    Header = new Header
                    {
                        Sender = new Sender
                        {
                            ServiceProvider = Sender
                        },
                        Receiver = new Receiver
                        {
                            ServiceProvider = Receiver
                        },
                        Timestamp = _timestamp
                    },
                    Body = new ContractTerminationRequestBody
                    {
                        Content = new ContractTerminationRequest
                        {
                            DossierId = dossierId,
                            RecipientServiceProvider = Sender,
                            DonorServiceProvider = Receiver,
                            Business = "N",
                            EarlyTermination = "N",
                            Name = "Someone Withaname",
                            AddressBlock = new AddressBlock
                            {
                                Housenr = "123",
                                HousenrExt = "A",
                                Postcode = "1234AB"
                            },
                            NumberSeries = new List<NumberSeries>
                            {
                                new NumberSeries
                                {
                                    Start = PhoneNumber,
                                    //End = PhoneNumber
                                }
                            }/*,
                            ValidationBlock = new List<ValidationBlock>
                            {
                                new ValidationBlock
                                {
                                    Name = "contractid",
                                    Value = "some contractid"
                                }
                            },
                            Note = "A note"*/
                        }
                    }
                }
            };
            var response = _contractTerminationService.SendMessageAsync(message).Result;
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
            var response = _contractTerminationService.SendConfirmationAsync(messageId).Result;
            Console.WriteLine(response.IsSuccessStatusCode
                ? $"Successfully sent confirmation of message {messageId}"
                : $"Confirmation failed with status {response.StatusCode}. Response: {response.Content.ReadAsStringAsync().Result}.");

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}