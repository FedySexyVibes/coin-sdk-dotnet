# COIN REST APIs

[![NuGet version](https://badge.fury.io/nu/Vereniging-COIN.Sdk.NP.svg)](https://badge.fury.io/nu/Vereniging-COIN.Sdk.NP)
[![CI Status](https://gitlab.com/verenigingcoin-public/coin-sdk-nodejs/badges/master/pipeline.svg)](https://gitlab.com/verenigingcoin-public/coin-sdk-nodejs/-/pipelines/latest)

| Api                 | SDK Version | Api Version    | Changelog |
|---------------------|-------------|----------------|----------------
| [number-portability](https://coin.nl/en/services/nummerportabiliteit) | 1.3.0       | [v3](https://api.coin.nl/docs/number-portability/v3)             | [click](CHANGELOG.md#version-1.3.0)
|                    | 0.0.1 - 1.2.1   | [v1](https://api.coin.nl/docs/number-portability/v1)             | -
| [bundle-switching](https://coin.nl/en/services/overstappen)   | 0.0.1 - 1.3.0         | [v4](https://api.coin.nl/docs/bundle-switching/v4)            | -

## Introduction

The COIN RESTful APIs serve as modern replacements for the outdated MQ and SOAP interfaces.
This new approach consists of highly secure yet quick and easy to implement web-standards thereby avoiding additional middleware for API users. 

In order to ease the introduce to the new APIs for new users, Vereniging COIN offers SDKs in several programming languages. 

For a quick start, follow the steps below:

* [Configure Credentials](#configure-credentials)
* [Choose an API](#choose-an-api)
* [Additional Resources](#additional-resources)
* [Support](#support)


## Configure Credentials

### Generate Keys

In order to be able to use a COIN API, the user is required to have a private/public key pair. Execute the following commands:
```
ssh-keygen -m PEM -t rsa -b 4096 -f private-key.pem -N '' 
ssh-keygen -e -m PKCS8 -f private-key.pem > public-key.pem
```

For Windows users, please note that running the commands in `git-bash` is required:
```
ssh-keygen.exe -m PEM -t rsa -b 4096 -f private-key.pem -N ''
ssh-keygen.exe -e -m PKCS8 -f private-key.pem > public-key.pem
```

These scripts generate the private and public keys: `public-key.pem` and `private-key.pem`.

### Store keys in COIN's IAM (Identity and Access Manager)

- Go to: https://test-portal.coin.nl/iam#/
    - Access to this site requires a user account that can be requested at [Coin Servicedesk](mailto:servicedesk@coin.nl)

- Select ***consumer name***
![alt text](./img/coin_iam_select_consumer.png "Select Consumer")

- Configure ***IP Addresses*** and add ***public key***
![alt text](./img/coin_iam_add_public_key.png "Configure IPs and public key")

- Press ***save***
![alt text](./img/coin_iam_save.png "Save")

- Retrieve ***client credentials***
    - Copy the contents of the ***Encrypted hmac secret*** field into a file named `sharedkey.encrypted`.
![alt text](./img/coin_iam_all_credentials.png "Retrieve Client Credentials")
 
Once the public key has been registered, the following are needed to gain access to the COIN APIs:
- `consumer name`
- `private-key.pem`
- `sharedkey.encrypted`

## Choose an API

- [Number Portability](number-portability-sdk/README.md)
- [Bundle Switching](bundle-switching-sdk/README.md)

## Additional Resources

### COIN APIs
- [Swagger-UI](https://test-api.coin.nl/docs)
- [Swagger-File](https://test-api.coin.nl/docs/number-portability/v1/swagger.json)
- [API Dashboard](https://test-portal.coin.nl/apis)
- [General info about accessing Coin APIs](https://gitlab.com/verenigingcoin-public/cpc-client)

## Support
If you need support, feel free to send an email to the [Coin devops team](mailto:devops@coin.nl).
