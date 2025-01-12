FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpliSeoSearch.BlazorApp/SimpliSeoSearch.BlazorApp.csproj", "SimpliSeoSearch.BlazorApp/"]
RUN dotnet restore "SimpliSeoSearch.BlazorApp/SimpliSeoSearch.BlazorApp.csproj"
COPY . .
WORKDIR "/src/SimpliSeoSearch.BlazorApp"
RUN dotnet build "SimpliSeoSearch.BlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpliSeoSearch.BlazorApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpliSeoSearch.BlazorApp.dll"]
