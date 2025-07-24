FROM nginx:latest AS base
USER $APP_UID
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILDPLATFORM
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["src/MadWorldNL.MadTransfer.Controllers.Web/Web.csproj", "MadWorldNL.MadTransfer.Controllers.Web/"]
RUN dotnet restore "MadWorldNL.MadTransfer.Controllers.Web/Web.csproj"
COPY ["src/", ""]
COPY ["src/MadWorldNL.MadTransfer.Controllers.Web/Configs/web/appsettings.json", "MadWorldNL.MadTransfer.Controllers.Web/wwwroot"]
WORKDIR /src/MadWorldNL.MadTransfer.Controllers.Web
RUN dotnet build "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
LABEL org.opencontainers.image.source="https://github.com/MadWorldNL/MadTransfer"

COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html
COPY ["src/MadWorldNL.MadTransfer.Controllers.Web/Configs/nginx/default.conf", "/etc/nginx/conf.d/"]