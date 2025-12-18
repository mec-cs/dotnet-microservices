FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Expose the Kestrel port used in compose
EXPOSE 5277

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Users.API/Users.API.csproj", "Users.API/"]
COPY ["DotNetMicroservices.ServiceDefaults/DotNetMicroservices.ServiceDefaults.csproj", "DotNetMicroservices.ServiceDefaults/"]
COPY ["Users.APP/Users.APP.csproj", "Users.APP/"]
COPY ["Core/Core.csproj", "Core/"]
RUN dotnet restore "Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Users.API"
RUN dotnet build "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]
