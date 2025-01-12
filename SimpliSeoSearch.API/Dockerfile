FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpliSeoSearch.API/SimpliSeoSearch.API.csproj", "SimpliSeoSearch.API/"]
COPY ["SimpliSeoSearch.Core/SimpliSeoSearch.Core.csproj", "SimpliSeoSearch.Core/"]
RUN dotnet restore "SimpliSeoSearch.API/SimpliSeoSearch.API.csproj"
COPY . .
WORKDIR "/src/SimpliSeoSearch.API"
RUN dotnet build "SimpliSeoSearch.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpliSeoSearch.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpliSeoSearch.API.dll"]
