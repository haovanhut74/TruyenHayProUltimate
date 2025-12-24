FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TruyenHayPro.Admin/TruyenHayPro.Admin/TruyenHayPro.Admin.csproj", "src/TruyenHayPro.Admin/TruyenHayPro.Admin/"]
COPY ["src/TruyenHayPro.Admin/TruyenHayPro.Admin.Client/TruyenHayPro.Admin.Client.csproj", "src/TruyenHayPro.Admin/TruyenHayPro.Admin.Client/"]
RUN dotnet restore "src/TruyenHayPro.Admin/TruyenHayPro.Admin/TruyenHayPro.Admin.csproj"
COPY . .
WORKDIR "/src/src/TruyenHayPro.Admin/TruyenHayPro.Admin"
RUN dotnet build "./TruyenHayPro.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TruyenHayPro.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TruyenHayPro.Admin.dll"]
