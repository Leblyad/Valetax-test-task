# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedModels/SharedModels.csproj SharedModels/
COPY Wallets/Wallets.API/Wallets.API.csproj Wallets/Wallets.API/
COPY Wallets/Wallets.Application/Wallets.Application.csproj Wallets/Wallets.Application/
COPY Wallets/Wallets.Domain/Wallets.Domain.csproj Wallets/Wallets.Domain/
COPY Wallets/Wallets.Infrastructure/Wallets.Infrastructure.csproj Wallets/Wallets.Infrastructure/
COPY Wallets/Wallets.Persistence/Wallets.Persistence.csproj Wallets/Wallets.Persistence/

RUN dotnet restore Wallets/Wallets.API/Wallets.API.csproj

COPY SharedModels/ SharedModels/
COPY Wallets/Wallets.API/ Wallets/Wallets.API/
COPY Wallets/Wallets.Application/ Wallets/Wallets.Application/
COPY Wallets/Wallets.Domain/ Wallets/Wallets.Domain/
COPY Wallets/Wallets.Infrastructure/ Wallets/Wallets.Infrastructure/
COPY Wallets/Wallets.Persistence/ Wallets/Wallets.Persistence/

RUN dotnet publish Wallets/Wallets.API/Wallets.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Wallets.API.dll"]
