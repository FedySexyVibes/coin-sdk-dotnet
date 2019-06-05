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
- A sample project is provided in the `number-portability-sdk-samples` directory.

## Configure Credentials

For secure access credentials are required.
- Check the [README introduction](../README.md#introduction) to find out how to configure these.
- In summary you will need:
    - a consumer name
    - a private key file (or a `System.Security.Cryptography.RSA` instance containing this key)
    - a file containing the encrypted Hmac secret
    (or a `System.Security.Cryptography.HMACSHA256` instance containing this (decrypted) secret)
- These can be used to create instances of the `NumberPortabilityService` and  the `NumberPortabilityMessageConsumer`.

## Messages

NpMessage.cs shows how the number portability domain has been structured.
The interfaces from this file can be used to define methods that can accept any message.
Coin.Sdk.NP.Messages.V1.Utils contains the useful static methods `TypeName` and `Deserialize`.

## Send Messages

The NumberPortabilityService has a `SendMessage` method to send any number portability message - wrapped in an instance of the MessageEnvelope class -
to the API.

## Consume Messages

An instance of the `INumberPortabilityMessageListener` needs to be passed to the `NumberPortabilityMessageConsumer` which will then start consuming messages.

By default, the `NumberPortabilityMessageConsumer` consumes all ***Unconfirmed*** messages. 


#### Consume specific messages using filters
The `NumberPortabilityMessageConsumer` provides various filters for message consumption. The filters are:
- `MessageType`: All possible message types, including errors. Use the `MessageType`-enumeration to indicate which messages have to be consumed.
- ConfirmationStatus: 
    - `ConfirmationStatus.Unconfirmed`: consumes all unconfirmed messages. Upon (re)-connection all unconfirmed messages are served.
    - `ConfirmationStatus.All`: consumes confirmed and unconfirmed messages.  
    **Note:** this filter enables the consumption of the *whole message history*.
    Therefore, this filter requires you to supply an implementation of the `IOffsetPersister` interface.
    The purpose of this interface is to track the `message-id` of the last received and processed message.
    In the case of a reconnect, message consumption will then resume where it left off.
- `offset`: starts consuming messages based on the given `message-id` offset.
When using `ConfirmationStatus.Unconfirmed` the `offset` is in most cases not very useful. The `ConfirmationStatus.All` filter is better.
***Note:*** it is the responsibility of the client to keep track of the `offset`.

#### Confirm Messages
Once a consumed message is processed it needs to be confirmed.
To confirm a message use the `NumberPortabilityService.sendConfirmation(id)` method.


## Error Handling

The number portability API can return errors in one of two ways:

1. The server received an incorrect payload and replies with an error response (synchronous)

    The REST layer of the system only performs basic payload checks, such as swagger schema validity and message authorization.
    Any errors in these checks are immediately returned as an error reply.
    The `NumberPortabilityService.SendMessage` method returns these as `ErrorResponse`.
    Other http errors are thrown as `HttpListenerException`.
    
2. As a ServerSentEvent containing an `ErrorFoundMessage` (asynchronous)

    The system performs detailed functional validation, such as number range validation etc. in an asynchronous fashion.
    Errors in this stage are sent asynchronously via a `ServerSentEvent`.
    ***Note:*** `ErrorFound` messages need to be confirmed like any other message received via `ServerSentEvent`.