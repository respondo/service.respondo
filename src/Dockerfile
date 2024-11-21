FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY *.sln ./

COPY Respondo.Api/*.csproj Respondo.Api/
COPY Respondo.Application/*.csproj Respondo.Application/
COPY Respondo.Core.Identity/*.csproj Respondo.Core.Identity/
COPY Respondo.Core.Identity.Contracts/*.csproj Respondo.Core.Identity.Contracts/
COPY Respondo.Core.Occasions/*.csproj Respondo.Core.Occasions/
COPY Respondo.Core.Occasions.Contracts/*.csproj Respondo.Core.Occasions.Contracts/
COPY Respondo.Core.Parties/*.csproj Respondo.Core.Parties/
COPY Respondo.Core.Parties.Contracts/*.csproj Respondo.Core.Parties.Contracts/
COPY Respondo.Core.Surveys/*.csproj Respondo.Core.Surveys/
COPY Respondo.Core.Surveys.Contracts/*.csproj Respondo.Core.Surveys.Contracts/
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
