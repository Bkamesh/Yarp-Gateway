FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
USER ${APP_UID}
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Dotnet.Helper Dotnet.Helper
COPY Yarp-Gateway/Yarp-Gateway Yarp-Gateway/Yarp-Gateway 
RUN dotnet restore Yarp-Gateway/Yarp-Gateway/Yarp-Gateway.csproj
RUN dotnet build Yarp-Gateway/Yarp-Gateway/Yarp-Gateway.csproj -c Release

FROM build AS publish
RUN dotnet publish Yarp-Gateway/Yarp-Gateway/Yarp-Gateway.csproj -o /app/publish -c Release

FROM base
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet","Yarp-Gateway.dll" ]

#docker build -f ./Yarp-Gateway/Yarp-Gateway/Dockerfile -t localhost:32000/yarp-gateway:1.0 .
#imageName: localhost:32000/yarp-gateway:1.0