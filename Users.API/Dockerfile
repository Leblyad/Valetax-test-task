# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Users.API/Users.API.csproj Users.API/
COPY Users.Application/Users.Application.csproj Users.Application/
COPY Users.Domain/Users.Domain.csproj Users.Domain/
COPY Users.Infrastructure/Users.Infrastructure.csproj Users.Infrastructure/
COPY Users.Persistence/Users.Persistence.csproj Users.Persistence/

RUN dotnet restore Users.API/Users.API.csproj

COPY Users.API/ Users.API/
COPY Users.Application/ Users.Application/
COPY Users.Domain/ Users.Domain/
COPY Users.Infrastructure/ Users.Infrastructure/
COPY Users.Persistence/ Users.Persistence/

RUN dotnet publish Users.API/Users.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Users.API.dll"]
