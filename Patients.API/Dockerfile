FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
# Expose the Kestrel port used in compose
EXPOSE 5063

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Patients.API/Patients.API.csproj", "Patients.API/"]
COPY ["Patients.APP/Patients.APP.csproj", "Patients.APP/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Users.APP/Users.APP.csproj", "Users.APP/"]
COPY ["DotNetMicroservices.ServiceDefaults/DotNetMicroservices.ServiceDefaults.csproj", "DotNetMicroservices.ServiceDefaults/"]
RUN dotnet restore "Patients.API/Patients.API.csproj"
COPY . .
WORKDIR "/src/Patients.API"
RUN dotnet build "./Patients.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Patients.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Patients.API.dll"]
