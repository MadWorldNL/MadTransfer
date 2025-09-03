FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG BUILDPLATFORM
RUN apt-get update
RUN apt-get --no-install-recommends install -y curl

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILDPLATFORM
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/MadWorldNL.MadTransfer.Controllers.Api/Api.csproj", "MadWorldNL.MadTransfer.Controllers.Api/"]
RUN dotnet restore "MadWorldNL.MadTransfer.Controllers.Api/Api.csproj"
COPY ["src/", "src/"]
WORKDIR /src/src/MadWorldNL.MadTransfer.Controllers.Api
RUN dotnet build "./Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadWorldNL.MadTransfer.Controllers.Api.dll"]
