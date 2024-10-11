FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY *.sln ./

COPY Respondo.Api/*.csproj Respondo.Api/
COPY Respondo.Core.Events/*.csproj Respondo.Core.Events/
COPY Respondo.Core.Events.Contracts/*.csproj Respondo.Core.Events.Contracts/
COPY Respondo.Core.Identity/*.csproj Respondo.Core.Identity/
COPY Respondo.Core.Identity.Contracts/*.csproj Respondo.Core.Identity.Contracts/
COPY Respondo.Persistence/*.csproj Respondo.Persistence/
COPY Respondo.Persistence.Migrations/*.csproj Respondo.Persistence.Migrations/
COPY Respondo.Testing.Integration/*.csproj Respondo.Testing.Integration/
COPY Respondo.Testing.Unit/*.csproj Respondo.Testing.Unit/

RUN dotnet restore -r linux-musl-x64

COPY . .
WORKDIR /src/Respondo
RUN dotnet publish -c Release -o /app -r linux-musl-x64 --self-contained false --no-restore ../Respondo.Api/Respondo.Api.csproj

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS http://*:5100
EXPOSE 5100

ENTRYPOINT ["dotnet", "Respondo.Api.dll"]
