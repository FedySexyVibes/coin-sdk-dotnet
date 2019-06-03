# Java Common SDK

## Introduction

The C# Common SDK contains the basics for secured access to all COIN APIs.
It contains no specific API implementation.

## Configure Credentials

For secure access credentials are required.
- Check the [README introduction](../README.md#introduction) to find out how to configure these.
- In summary you will need:
    - a consumer name
    - a private key file (or a `System.Security.Cryptography.RSA` instance containing this key)
    - a file containing the encrypted Hmac secret
    (or a `System.Security.Cryptography.HMACSHA256` instance containing this (decrypted) secret)
- These can be used to create instances of the `CoinHttpClientHandler` and `CtpApiRestTemplateSupport`.