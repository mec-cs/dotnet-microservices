FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Expose the Kestrel port used in compose
EXPOSE 5168

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Locations.API/Locations.API.csproj", "Locations.API/"]
COPY ["DotNetMicroservices.ServiceDefaults/DotNetMicroservices.ServiceDefaults.csproj", "DotNetMicroservices.ServiceDefaults/"]
COPY ["Locations.APP/Locations.APP.csproj", "Locations.APP/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Patients.APP/Patients.APP.csproj", "Patients.APP/"]
COPY ["Users.APP/Users.APP.csproj", "Users.APP/"]
RUN dotnet restore "Locations.API/Locations.API.csproj"
COPY . .
WORKDIR "/src/Locations.API"
RUN dotnet build "./Locations.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Locations.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Locations.API.dll"]
