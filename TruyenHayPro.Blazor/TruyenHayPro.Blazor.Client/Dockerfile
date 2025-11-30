FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TruyenHayPro.Blazor/TruyenHayPro.Blazor.Client/TruyenHayPro.Blazor.Client.csproj", "TruyenHayPro.Blazor/TruyenHayPro.Blazor.Client/"]
RUN dotnet restore "TruyenHayPro.Blazor/TruyenHayPro.Blazor.Client/TruyenHayPro.Blazor.Client.csproj"
COPY . .
WORKDIR "/src/TruyenHayPro.Blazor/TruyenHayPro.Blazor.Client"
RUN dotnet build "./TruyenHayPro.Blazor.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TruyenHayPro.Blazor.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TruyenHayPro.Blazor.Client.dll"]
