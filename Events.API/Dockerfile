# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Events.API/Events.API.csproj Events.API/
COPY Events.Application/Events.Application.csproj Events.Application/
COPY Events.Domain/Events.Domain.csproj Events.Domain/
COPY Events.Infrastructure/Events.Infrastructure.csproj Events.Infrastructure/
COPY Events.Persistence/Events.Persistence.csproj Events.Persistence/

RUN dotnet restore Events.API/Events.API.csproj

COPY Events.API/ Events.API/
COPY Events.Application/ Events.Application/
COPY Events.Domain/ Events.Domain/
COPY Events.Infrastructure/ Events.Infrastructure/
COPY Events.Persistence/ Events.Persistence/

RUN dotnet publish Events.API/Events.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Events.API.dll"]
