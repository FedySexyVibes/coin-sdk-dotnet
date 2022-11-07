using System.Collections.Generic;
using Coin.Sdk.BS.Messages.V5;
using Coin.Sdk.BS.Service.Impl;
using NUnit.Framework;
using static Coin.Sdk.BS.Tests.TestSettings;
using static Coin.Sdk.BS.Tests.TestUtils;

namespace Coin.Sdk.BS.Tests
{
    public class BundleSwitchingServiceTests
    {
        private BundleSwitchingService _bundleSwitchingService = null!;

        [SetUp]
        public void Setup()
        {
            _bundleSwitchingService = new BundleSwitchingService(ApiUrl, Consumer, PrivateKeyFile, EncryptedHmacSecretFile);
        }

        private MessageResponse SendContractTerminationRequest(string dossierId)
        {
            var message = new MessageEnvelope<ContractTerminationRequest>
            {
                Message = new ContractTerminationRequestMessage
                {
                    Header = new Header
                    {
                        Sender = new Sender
                        {
                            ServiceProvider = Recipient
                        },
                        Receiver = new Receiver
                        {
                            ServiceProvider = Donor
                        },
                        Timestamp = Timestamp
                    },
                    Body = new ContractTerminationRequestBody
                    {
                        Content = new ContractTerminationRequest
                        {
                            DossierId = dossierId,
                            RecipientServiceProvider = Recipient,
                            DonorServiceProvider = Donor,
                            EarlyTermination = "N",
                            Name = "Vereniging COIN",
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
                                    End = PhoneNumber
                                }
                            },
                            ValidationBlock = new List<ValidationBlock>
                            {
                                new ValidationBlock
                                {
                                    Name = "sdfn33-fdin",
                                    Value = "gdsf-45n"
                                }
                            },
                            Note = "Some note"
                        }
                    }
                }
            };
            return _bundleSwitchingService.SendMessageAsync(message).Result;
        }

        [Test]
        public void SendContractTerminationRequest()
        {
            var dossierId = GenerateDossierId(Recipient, Donor);
            var response = SendContractTerminationRequest(dossierId);
            Assert.IsNotInstanceOf<ErrorResponse>(response);
        }

        [Test]
        public void SendInvalidContractTerminationRequest()
        {
            var dossierId = GenerateDossierId(Recipient, Donor);
            var response = SendContractTerminationRequest(dossierId + "invalid");
            Assert.IsInstanceOf<ErrorResponse>(response);
        }
    }
}