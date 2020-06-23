# How to manage the NuGet packages
###### Instructions for COIN developers

### Gaining access
The packages are published at NuGet.org using the NuGet organization `VerenigingCOIN`.
If you need access (e.g. for regenerating the api key), you have to:
- create a NuGet.org account by signing in using your Microsoft @coin.nl account;
- ask an administrator of the NuGet organization `VerenigingCOIN` to add you as an administrator.

### Regenerating the api key
The api key is needed for pushing new versions of the package using the dotnet cli.
When it is about to expire, it has to be regenerated. This can be done at https://www.nuget.org/account/apikeys.
After regenerating this key, copy its value into the parameter `/alm/nuget/api_key` in the parameter store in git-api.