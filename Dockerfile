FROM pomma89/dotnet-mono:dotnet-3-mono-6-sdk
WORKDIR /app

COPY coin-sdk-dotnet.sln ./
COPY common-sdk/ ./common-sdk/
COPY number-portability-sdk/ ./number-portability-sdk/
COPY number-portability-sdk-tests/ ./number-portability-sdk-tests/

RUN dotnet build common-sdk && dotnet build number-portability-sdk
CMD dotnet test number-portability-sdk-tests