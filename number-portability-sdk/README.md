# .NET Number Portability SDK

## Introduction

This SDK supports secured access to the number portability API.

For a quick start, follow the steps below:
* [Setup](#setup)
* [Configure Credentials](#configure-credentials)
* [Send Messages](#send-messages)
* [Consume Messages](#consume-messages)
* [Error handling](#error-handling)


## Setup

### Sample Project for the Number Portability API
A sample project is provided in the `number-portability-sdk-samples` directory.

### NuGet Package
This sdk is published as the NuGet package 'Vereniging-COIN.Sdk.NP'.

## Configure Credentials

For secure access credentials are required.
- Check the [README introduction](../README.md#introduction) to find out how to configure these.
- In summary you will need:
    - a consumer name
    - a private key file (or a `System.Security.Cryptography.RSA` instance containing this key)
    - a file containing the encrypted Hmac secret
    (or a `System.Security.Cryptography.HMACSHA256` instance containing this (decrypted) secret)
- These can be used to create instances of the `NumberPortabilityService` and the `SseConsumer`, which is needed for the `NumberPortabilityMessageConsumer`.

## Messages

NpMessage.cs shows how the number portability domain has been structured.
The interfaces from this file can be used to define methods that can accept any message.
Coin.Sdk.NP.Messages.V1.Utils contains the useful static methods `TypeName` and `Deserialize`.

## Send Messages

The NumberPortabilityService has a `SendMessage` method to send any number portability message - wrapped in an instance of the MessageEnvelope class -
to the API.

## Consume Messages

### Create Message Listener
For message consumption, the number portability API makes use of HTTP's [ServerSentEvents](https://en.wikipedia.org/wiki/Server-sent_events).
The SDK offers a Listener interface `INumberPortabilityMessageListener` which is triggered upon reception of a message payload.

### Start consuming messages 
The `NumberPortabilityMessageConsumer` has three `startConsuming...()` methods for consuming messages, of which `startConsumingUnconfirmed()` is most useful.
All these methods need an instance of the `INumberPortabilityMessageListener`.

### Consume specific messages using filters
The `NumberPortabilityMessageConsumer` provides various filters for message consumption. The filters are:
- `MessageType`: All possible message types, including errors. Use the `MessageType`-enumeration to indicate which messages have to be consumed.
- confirmation status: By using `startConsumingAll()`, all messages will be received, both confirmed and unconfirmed.   
    **Note:** this enables the consumption of the *whole message history*.
    Therefore, this requires you to supply an implementation of the `IOffsetPersister` interface.
    The purpose of this interface is to track the `message-id` of the last received and processed message.
    In the case of a reconnect, message consumption will then resume where it left off.
- `offset`: starts consuming messages based on the given `message-id` offset. ***Note:*** it is the responsibility of the client to keep track of the `offset`.

#### Confirm Messages
Once a consumed message has been processed it needs to be confirmed.
To confirm a message use the `NumberPortabilityService.sendConfirmation(id)` method.


## Error Handling

The number portability API can return errors in one of two ways:

1. The server received an incorrect payload and replies with an error response (synchronous)

    The REST layer of the system only performs basic payload checks, such as swagger schema validity and message authorization.
    Any errors in these checks are immediately returned as an error reply.
    The `NumberPortabilityService.SendMessage` method returns these as `ErrorResponse`.
    Other http errors are thrown as `HttpListenerException`.

2. As a ServerSentEvent containing an `ErrorFoundMessage` (asynchronous)

    The system performs detailed functional validation, such as number range validation etc, asynchronously.
    Errors in this stage are sent via a `ServerSentEvent`.

    ***Note:*** `ErrorFound` messages need to be confirmed like any other message received via a `ServerSentEvent`.