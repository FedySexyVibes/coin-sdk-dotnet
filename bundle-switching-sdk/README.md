# .NET Bundle Switching SDK

## Introduction

This SDK supports secured access to the bundle switching API.

For a quick start, follow the steps below:
* [Setup](#setup)
* [Configure Credentials](#configure-credentials)
* [Send Messages](#send-messages)
* [Consume Messages](#consume-messages)
* [Error handling](#error-handling)


## Setup

### Sample Project for the Bundle Switching API
A sample project is provided in the `bundle-switching-sdk-samples` directory.

### NuGet Package
This SDK is published as the NuGet package 'Vereniging-COIN.Sdk.BS'.

## Configure Credentials

For secure access credentials are required.
- Check [this README](https://gitlab.com/verenigingcoin-public/consumer-configuration/-/blob/master/README.md) to find out how to configure these.
- In summary you will need:
    - a consumer name
    - a private key file (or a `System.Security.Cryptography.RSA` instance containing this key)
    - a file containing the encrypted Hmac secret
    (or a `System.Security.Cryptography.HMACSHA256` instance containing this (decrypted) secret)
- These can be used to create instances of the `BundleSwitchingService` and the `SseConsumer`, which is needed for the `BundleSwitchingMessageConsumer`.

## Messages

BsMessage.cs shows how the bundle switching domain has been structured.
The interfaces from this file can be used to define methods that can accept any message.
Coin.Sdk.BS.Messages.V5.Utils contains the useful static methods `TypeName` and `Deserialize`.

## Send Messages

The BundleSwitchingService has a `SendMessage` method to send any bundle switching message - wrapped in an instance of the MessageEnvelope class -
to the API.

## Consume Messages

### Create Message Listener
For message consumption, the bundle switching API makes use of HTTP's [ServerSentEvents](https://en.wikipedia.org/wiki/Server-sent_events).
The SDK offers a Listener interface `IBundleSwitchingMessageListener` which is triggered upon reception of a message payload.
Whenever the API doesn't send any other message for 20 seconds, it sends an empty 'heartbeat' message, which triggers the OnKeepAlive() method.

### Start consuming messages 
The `BundleSwitchingMessageConsumer` has three `startConsuming...()` methods for consuming messages, of which `startConsumingUnconfirmed()` is most useful.
All these methods need an instance of the `IBundleSwitchingMessageListener`.

### Consume specific messages using filters
The `BundleSwitchingMessageConsumer` provides various filters for message consumption. The filters are:
- `MessageType`: All possible message types, including errors. Use the `MessageType`-enumeration to indicate which messages have to be consumed.
- confirmation status: By using `startConsumingAll()`, all messages will be received, both confirmed and unconfirmed.   
    **Note:** this enables the consumption of the *whole message history*.
    Therefore, this requires you to supply an implementation of the `IOffsetPersister` interface.
    The purpose of this interface is to track the `message-id` of the last received and processed message.
    In the case of a reconnect, message consumption will then resume where it left off.
- `offset`: starts consuming messages based on the given `message-id` offset. ***Note:*** it is the responsibility of the client to keep track of the `offset`.

#### Confirm Messages
Once a consumed message has been processed it needs to be confirmed.
To confirm a message use the `BundleSwitchingService.sendConfirmation(id)` method.


## Error Handling

The bundle switching API can return errors in one of two ways:

1. The server received an incorrect payload and replies with an error response (synchronous)

    The REST layer of the system only performs basic payload checks, such as swagger schema validity and message authorization.
    Any errors in these checks are immediately returned as an error reply.
    The `BundleSwitchingService.SendMessage` method returns these as `ErrorResponse`.
    Other http errors are thrown as `HttpListenerException`.

2. As a ServerSentEvent containing an `ErrorFoundMessage` (asynchronous)

    The system performs detailed functional validation asynchronously.
    Errors in this stage are sent via a `ServerSentEvent`.

    ***Note:*** `ErrorFound` messages need to be confirmed like any other message received via a `ServerSentEvent`.