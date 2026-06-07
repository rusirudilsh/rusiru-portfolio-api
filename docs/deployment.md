# Deployment

## Overview

This document outlines how I plan to deploy the Rusiru Portfolio API.

The API will initially be deployed using a simple and maintainable Azure setup. Since this is a portfolio API, I do not want to over-engineer the first deployment with unnecessary infrastructure. The goal is to keep the deployment reliable, secure, easy to maintain, and easy to extend later.

---

## Initial Deployment Plan

For the first production version, I plan to use:

```text
Azure App Service     - Host the ASP.NET Core API
Azure SQL Database    - Store portfolio content
GitHub Actions        - Build, test, and deploy the API
Application Settings  - Store environment-specific configuration
Application Insights  - Monitor API logs, errors, and performance
```

Azure App Service is enough for the initial version because this API is a standard ASP.NET Core Web API and does not currently need container orchestration.

---

## Local Deployment

For local development, the API can be run using Docker Compose:

```bash
docker compose up --build
```

This starts:

```text
ASP.NET Core API
SQL Server database
```

The API should be available at:

```text
http://localhost:5000
```

---

## Production Configuration

Production configuration will be stored outside the source code.

Examples:

```text
ASPNETCORE_ENVIRONMENT
ConnectionStrings__DefaultConnection
AllowedCorsOrigins
Jwt__Issuer
Jwt__Audience
Jwt__Secret
```

Secrets and production connection strings must not be committed to GitHub.

For Azure deployment, these values will be stored using:

```text
Azure App Service Application Settings
Azure Key Vault, if needed later
```

---

## Database Deployment

The production database will use Azure SQL Database.

Entity Framework Core migrations will be used to manage database schema changes.

Migration example:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Rusiru.Portfolio.Infrastructure \
  --startup-project src/Rusiru.Portfolio.Api
```

Apply migration:

```bash
dotnet ef database update \
  --project src/Rusiru.Portfolio.Infrastructure \
  --startup-project src/Rusiru.Portfolio.Api
```

For production, I will apply migrations carefully as part of the deployment process because database changes can be harder to roll back than application code.

---

## CI/CD Plan

The deployment pipeline will use GitHub Actions.

Planned pipeline steps:

```text
1. Restore NuGet packages
2. Build the solution
3. Run tests
4. Publish the API
5. Deploy to Azure App Service
6. Verify the deployment using the health check endpoint
```

The pipeline should fail if the build or tests fail.

---

## Health Check

The API exposes a health check endpoint:

```text
GET /api/health
```

Expected response:

```text
Healthy
```

This endpoint will be used to confirm that the deployed API is running successfully.

Example:

```bash
curl https://your-api-domain.com/api/health
```

---

## Deployment Verification

After deployment, I will verify:

```text
/api/health returns Healthy
API starts without errors
Database connection works
Application logs are available
CORS allows only expected origins
Admin endpoints are protected
Production secrets are not exposed
```

---

## Future Improvements

Future deployment improvements may include:

```text
Azure Container Apps if I want a more container-focused deployment
Azure Key Vault for stronger secret management
Deployment slots for safer releases
Azure Front Door or WAF for additional edge protection
Custom domain and HTTPS configuration
Post-deployment smoke tests in GitHub Actions
```

For the initial version, Azure App Service is the preferred option because it is simple, reliable, and suitable for this API.
