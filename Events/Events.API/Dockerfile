# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SharedModels/SharedModels.csproj SharedModels/
COPY Events/Events.API/Events.API.csproj Events/Events.API/
COPY Events/Events.Application/Events.Application.csproj Events/Events.Application/
COPY Events/Events.Domain/Events.Domain.csproj Events/Events.Domain/
COPY Events/Events.Infrastructure/Events.Infrastructure.csproj Events/Events.Infrastructure/
COPY Events/Events.Persistence/Events.Persistence.csproj Events/Events.Persistence/

RUN dotnet restore Events/Events.API/Events.API.csproj

COPY SharedModels/ SharedModels/
COPY Events/Events.API/ Events/Events.API/
COPY Events/Events.Application/ Events/Events.Application/
COPY Events/Events.Domain/ Events/Events.Domain/
COPY Events/Events.Infrastructure/ Events/Events.Infrastructure/
COPY Events/Events.Persistence/ Events/Events.Persistence/

RUN dotnet publish Events/Events.API/Events.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Events.API.dll"]
