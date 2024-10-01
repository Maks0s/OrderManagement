FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotificationService/src/NotificationService.Host/NotificationService.Host.csproj", "NotificationService/src/NotificationService.Host/"]
COPY ["NotificationService/src/NotificationService.Application/NotificationService.Application.csproj", "NotificationService/src/NotificationService.Application/"]
COPY ["Shared.Contracts/Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/Shared.Contracts/"]
COPY ["NotificationService/src/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService/src/NotificationService.Infrastructure/"]
RUN dotnet restore "./NotificationService/src/NotificationService.Host/NotificationService.Host.csproj"
COPY . .
WORKDIR "/src/NotificationService/src/NotificationService.Host"
RUN dotnet build "./NotificationService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotificationService.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Host.dll"]