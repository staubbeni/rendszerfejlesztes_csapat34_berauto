# Base stage for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080



# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy project files for restore
COPY ["BerAuto/BerAuto.csproj", "BerAuto/"]
COPY ["BerAuto.DataContext/BerAuto.DataContext.csproj", "BerAuto.DataContext/"]
COPY ["BerAuto.Service/BerAuto.Service.csproj", "BerAuto.Service/"]
RUN dotnet restore "BerAuto/BerAuto.csproj"
# Copy all source code
COPY ["BerAuto", "BerAuto/"]
COPY ["BerAuto.DataContext", "BerAuto.DataContext/"]
COPY ["BerAuto.Service", "BerAuto.Service/"]
WORKDIR "/src/BerAuto"
RUN dotnet build "BerAuto.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BerAuto.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BerAuto.dll"]