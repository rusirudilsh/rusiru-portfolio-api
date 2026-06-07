# Rusiru Portfolio API

ASP.NET Core Web API for managing and serving content for my personal developer portfolio.

This project is built using a lightweight Clean Architecture approach to demonstrate backend development, API design, SQL Server integration, Docker-based local development, testing, deployment planning, and maintainable project structure.

---

## Tech Stack

* ASP.NET Core Web API
* C# / .NET 8
* Entity Framework Core
* SQL Server
* Docker / Docker Compose
* Swagger / OpenAPI
* xUnit for testing

---

## Project Purpose

This API serves portfolio content such as:

* Hero section content
* About me / professional summary
* Technical skills
* Work experience
* Projects
* Recommendations
* Certifications
* Contact messages
* Admin-managed portfolio content

The API is intended to be consumed by a separate Vue.js frontend application.

---

## Project Structure

```text
Rusiru.Portfolio
│
├── src/
│   ├── Rusiru.Portfolio.Api/
│   ├── Rusiru.Portfolio.Application/
│   ├── Rusiru.Portfolio.Domain/
│   └── Rusiru.Portfolio.Infrastructure/
│
├── tests/
│   └── Rusiru.Portfolio.UnitTests/
│
├── docs/
│   ├── requirements.md
│   ├── system-design.md
│   ├── api-design.md
│   ├── database-design.md
│   ├── testing-strategy.md
│   └── deployment.md
│
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .gitignore
├── .env.example
└── README.md
```

---

## Documentation

Detailed project documentation is available in the `docs/` folder.

| Document                                               | Description                                          |
| ------------------------------------------------------ | ---------------------------------------------------- |
| [`docs/requirements.md`](docs/requirements.md)         | Functional and non-functional requirements           |
| [`docs/system-design.md`](docs/system-design.md)       | High-level system design and architectural decisions |
| [`docs/api-design.md`](docs/api-design.md)             | Planned API endpoints and API structure              |
| [`docs/database-design.md`](docs/database-design.md)   | Database design and entity relationships             |
| [`docs/testing-strategy.md`](docs/testing-strategy.md) | Testing approach for the API                         |
| [`docs/deployment.md`](docs/deployment.md)             | API deployment approach                              |

---

## Project Layers

### `Rusiru.Portfolio.Api`

API entry point.

Responsible for:

* Controllers
* Middleware
* Swagger/OpenAPI configuration
* Dependency injection setup
* Authentication and authorization setup
* Health check endpoint configuration
* HTTP request/response handling

### `Rusiru.Portfolio.Application`

Application/use-case layer.

Responsible for:

* DTOs
* Interfaces
* Application services
* Validation logic
* Business use cases
* Request/response models

### `Rusiru.Portfolio.Domain`

Core domain layer.

Responsible for:

* Domain entities
* Enums
* Domain rules
* Core business models

### `Rusiru.Portfolio.Infrastructure`

Infrastructure layer.

Responsible for:

* Entity Framework Core
* SQL Server database access
* DbContext
* Database migrations
* External service implementations
* Repository implementations, if required

### `Rusiru.Portfolio.UnitTests`

Unit test project.

Responsible for:

* Application service tests
* Domain logic tests
* Validation tests
* Unit-level behaviour testing

---

## Prerequisites

To run this project locally, install:

* [.NET SDK 8](https://dotnet.microsoft.com/)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* Git
* Optional: Visual Studio 2022, Rider, or VS Code

---

## Running the Project with Docker

From the repository root, run:

```bash
docker compose up --build
```

This starts:

* ASP.NET Core API container
* SQL Server container

The API should be available at:

```text
http://localhost:5000
```

Swagger should be available at:

```text
http://localhost:5000/swagger
```

---

## Health Check Endpoint

The API exposes a health check endpoint using the built-in ASP.NET Core health check middleware.

```text
GET /api/health
```

Expected response:

```text
Healthy
```

Expected status:

```text
200 OK
```

The endpoint is registered in `Program.cs` using:

```csharp
builder.Services.AddHealthChecks();

app.MapHealthChecks("/api/health");
```

Because this endpoint is registered through middleware and not implemented as a controller action, it may not appear in the Swagger/OpenAPI documentation. This is expected.

Test with:

```bash
curl http://localhost:5000/api/health
```

---

## Stopping Docker Containers

To stop the running containers:

```bash
docker compose down
```

To stop containers and remove the SQL Server volume/data:

```bash
docker compose down -v
```

Use `-v` carefully because it deletes the local database volume.

---

## Running Without Docker

From the repository root:

```bash
dotnet restore
dotnet build
dotnet run --project src/Rusiru.Portfolio.Api/Rusiru.Portfolio.Api.csproj
```

---

## Running Tests

From the repository root:

```bash
dotnet test
```

Or run the unit test project directly:

```bash
dotnet test tests/Rusiru.Portfolio.UnitTests/Rusiru.Portfolio.UnitTests.csproj
```

---

## Docker Setup

The project includes:

```text
Dockerfile
docker-compose.yml
.dockerignore
.env.example
```

### Dockerfile

Used to build and run the ASP.NET Core API inside a container.

### docker-compose.yml

Used to run the API and SQL Server together locally.

### .dockerignore

Used to prevent unnecessary files from being copied into the Docker build context.

Important ignored folders include:

```text
**/bin/
**/obj/
.git/
.vs/
```

This avoids Docker build issues caused by local Windows build artifacts being copied into the Linux container.

---

## Environment Variables

A sample environment file is provided:

```text
.env.example
```

Create a local `.env` file if needed:

```bash
cp .env.example .env
```

Never commit real secrets or production credentials to Git.

---

## Database

The local Docker setup uses SQL Server.

Example local connection string:

```text
Server=localhost,1433;Database=RusiruPortfolioDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;
```

Inside Docker, the API connects to SQL Server using the Docker service name:

```text
Server=portfolio-db,1433;Database=RusiruPortfolioDb;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;Encrypt=False
```

---

## Entity Framework Core Migrations

Add a migration:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Rusiru.Portfolio.Infrastructure \
  --startup-project src/Rusiru.Portfolio.Api
```

Update the database:

```bash
dotnet ef database update \
  --project src/Rusiru.Portfolio.Infrastructure \
  --startup-project src/Rusiru.Portfolio.Api
```

If `dotnet ef` is not installed:

```bash
dotnet tool install --global dotnet-ef
```

---

## Development Commands

Restore packages:

```bash
dotnet restore
```

Build solution:

```bash
dotnet build
```

Run API:

```bash
dotnet run --project src/Rusiru.Portfolio.Api/Rusiru.Portfolio.Api.csproj
```

Run tests:

```bash
dotnet test
```

Run with Docker:

```bash
docker compose up --build
```

Stop Docker containers:

```bash
docker compose down
```

Remove Docker containers and volumes:

```bash
docker compose down -v
```

---

## Repository Hygiene

This repository uses both:

```text
.gitignore
.dockerignore
```

### `.gitignore`

Prevents local build files, secrets, IDE files, and generated artifacts from being committed to GitHub.

### `.dockerignore`

Prevents unnecessary local files from being copied into the Docker image build context.

Both files are required because they solve different problems.

---

## Author

Rusiru Dilshan - Software Engineer


---

## License

This project is for personal portfolio and learning purposes.
