# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for better Docker layer caching
COPY src/Rusiru.Portfolio.Api/Rusiru.Portfolio.Api.csproj src/Rusiru.Portfolio.Api/
COPY src/Rusiru.Portfolio.Application/Rusiru.Portfolio.Application.csproj src/Rusiru.Portfolio.Application/
COPY src/Rusiru.Portfolio.Domain/Rusiru.Portfolio.Domain.csproj src/Rusiru.Portfolio.Domain/
COPY src/Rusiru.Portfolio.Infrastructure/Rusiru.Portfolio.Infrastructure.csproj src/Rusiru.Portfolio.Infrastructure/

RUN dotnet restore src/Rusiru.Portfolio.Api/Rusiru.Portfolio.Api.csproj

# Copy everything else
COPY . .

RUN dotnet publish src/Rusiru.Portfolio.Api/Rusiru.Portfolio.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Rusiru.Portfolio.Api.dll"]