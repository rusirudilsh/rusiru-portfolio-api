# Testing Strategy

## Overview

This document outlines how I plan to test the Rusiru Portfolio API.

The goal is to make sure the API is reliable, maintainable, and safe to change as new portfolio features are added.

---

## Testing Scope

The API will be tested at the following levels:

```text
Unit tests
Integration tests
Manual API tests
CI pipeline tests
```

---

## Unit Testing

Unit tests will focus on small pieces of application logic.

Areas to test:

```text
Application services
Domain rules
Validation logic
Mapping logic
Helper methods
```

Unit tests should not depend on a real database or external services.

Test project:

```text
tests/Rusiru.Portfolio.UnitTests
```

Run unit tests:

```bash
dotnet test
```

---

## Integration Testing

Integration tests will be added later to verify that the API works correctly with real dependencies.

Areas to test:

```text
API endpoints
Entity Framework Core queries
Database persistence
Authentication and authorization
Contact message submission
```

These tests may use a test database or containerized SQL Server.

---

## Manual API Testing

During development, I will manually test endpoints using:

```text
Swagger
Postman
curl
```

The health check endpoint can be tested with:

```bash
curl http://localhost:5000/api/health
```

Expected response:

```text
Healthy
```

---

## Test Cases to Cover

Initial test cases:

```text
Health check returns 200 OK
Public portfolio endpoints return expected data
Invalid requests return validation errors
Contact message endpoint validates required fields
Admin endpoints reject unauthenticated requests
Admin endpoints allow authenticated admin users
```

---

## CI Testing

The GitHub Actions pipeline should run tests before deployment.

Planned CI checks:

```text
Restore packages
Build solution
Run tests
Fail pipeline if tests fail
```

Example commands:

```bash
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

---

## Future Improvements

Future testing improvements may include:

```text
Integration tests with Testcontainers
Code coverage reports
API contract tests
Security-focused tests for admin endpoints
Post-deployment smoke tests
```
