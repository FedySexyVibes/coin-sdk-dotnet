FROM microsoft/dotnet:2.2-sdk
WORKDIR /app

COPY coin-sdk-dotnet.sln ./
COPY common-sdk/ ./common-sdk/
COPY number-portability-sdk/ ./number-portability-sdk/
COPY number-portability-sdk-tests/ ./number-portability-sdk-tests/
COPY bundle-switching-sdk/ ./bundle-switching-sdk/
COPY bundle-switching-sdk-tests/ ./bundle-switching-sdk-tests/
COPY keys/ ./keys/

RUN dotnet build common-sdk && dotnet build number-portability-sdk && dotnet build bundle-switching-sdk
CMD dotnet test number-portability-sdk-tests && dotnet test bundle-switching-sdk-tests