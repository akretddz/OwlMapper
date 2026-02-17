# syntax=docker/dockerfile:1

ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

COPY ["src/Bootstrapper/Bootstrapper.csproj", "src/Bootstrapper/"]
COPY ["Directory.Packages.props", "src"]
COPY ["src/Modules/Account/Account.csproj", "src/Modules/Account/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
RUN dotnet restore "src/Bootstrapper/Bootstrapper.csproj"

COPY . .
WORKDIR /src/src/Bootstrapper
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "Bootstrapper.dll"]