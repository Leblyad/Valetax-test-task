# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedModels/SharedModels.csproj SharedModels/
COPY Wallets.API/Wallets.API.csproj Wallets.API/
COPY Wallets.Application/Wallets.Application.csproj Wallets.Application/
COPY Wallets.Domain/Wallets.Domain.csproj Wallets.Domain/
COPY Wallets.Infrastructure/Wallets.Infrastructure.csproj Wallets.Infrastructure/
COPY Wallets.Persistence/Wallets.Persistence.csproj Wallets.Persistence/

RUN dotnet restore Wallets.API/Wallets.API.csproj

COPY SharedModels/ SharedModels/
COPY Wallets.API/ Wallets.API/
COPY Wallets.Application/ Wallets.Application/
COPY Wallets.Domain/ Wallets.Domain/
COPY Wallets.Infrastructure/ Wallets.Infrastructure/
COPY Wallets.Persistence/ Wallets.Persistence/

RUN dotnet publish Wallets.API/Wallets.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Wallets.API.dll"]
