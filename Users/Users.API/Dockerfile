# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedModels/SharedModels.csproj SharedModels/
COPY Users/Users.API/Users.API.csproj Users/Users.API/
COPY Users/Users.Application/Users.Application.csproj Users/Users.Application/
COPY Users/Users.Domain/Users.Domain.csproj Users/Users.Domain/
COPY Users/Users.Infrastructure/Users.Infrastructure.csproj Users/Users.Infrastructure/
COPY Users/Users.Persistence/Users.Persistence.csproj Users/Users.Persistence/

RUN dotnet restore Users/Users.API/Users.API.csproj

COPY SharedModels/ SharedModels/
COPY Users/Users.API/ Users/Users.API/
COPY Users/Users.Application/ Users/Users.Application/
COPY Users/Users.Domain/ Users/Users.Domain/
COPY Users/Users.Infrastructure/ Users/Users.Infrastructure/
COPY Users/Users.Persistence/ Users/Users.Persistence/

RUN dotnet publish Users/Users.API/Users.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Users.API.dll"]
