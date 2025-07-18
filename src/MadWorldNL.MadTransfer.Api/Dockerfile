FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "src/"]
COPY ["Directory.Packages.props", "src/"]
COPY ["src/MadWorldNL.MadTransfer.Api/MadWorldNL.MadTransfer.Api.csproj", "src/MadWorldNL.MadTransfer.Api/"]
RUN dotnet restore "src/MadWorldNL.MadTransfer.Api/MadWorldNL.MadTransfer.Api.csproj"
COPY . .
WORKDIR "/src/src/MadWorldNL.MadTransfer.Api"
RUN dotnet build "./MadWorldNL.MadTransfer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MadWorldNL.MadTransfer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadWorldNL.MadTransfer.Api.dll"]
