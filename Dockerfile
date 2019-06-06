FROM microsoft/dotnet:2.0-sdk
WORKDIR /app

COPY  coin-sdk-dotnet.sln ./
COPY  common-sdk/ ./common-sdk/
COPY  number-portability-sdk/ ./number-portability-sdk/

RUN dotnet build common-sdk
RUN dotnet build number-portability-sdk