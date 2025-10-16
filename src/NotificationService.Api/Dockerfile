# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/NotificationService.Api/NotificationService.Api.csproj", "src/NotificationService.Api/"]
COPY ["src/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "src/NotificationService.Infrastructure/"]
COPY ["src/NotificationService.Domain/NotificationService.Domain.csproj", "src/NotificationService.Domain/"]
RUN dotnet restore "src/NotificationService.Api/NotificationService.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/NotificationService.Api"
RUN dotnet build "NotificationService.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "NotificationService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]
