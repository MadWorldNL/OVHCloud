FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MadWorldNL.OVHCloud.DynHost.Web/MadWorldNL.OVHCloud.DynHost.Web.csproj", "MadWorldNL.OVHCloud.DynHost.Web/"]
COPY ["src/MadWorldNL.OVHCloud.DynHost.Lib/MadWorldNL.OVHCloud.DynHost.Lib.csproj", "MadWorldNL.OVHCloud.DynHost.Lib/"]
RUN dotnet restore "MadWorldNL.OVHCloud.DynHost.Web/MadWorldNL.OVHCloud.DynHost.Web.csproj"
COPY ["src/", "./"]
WORKDIR "/src/MadWorldNL.OVHCloud.DynHost.Web"
RUN dotnet build "./MadWorldNL.OVHCloud.DynHost.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MadWorldNL.OVHCloud.DynHost.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadWorldNL.OVHCloud.DynHost.Web.dll"]
