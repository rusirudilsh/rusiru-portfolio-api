# Rusiru Portfolio API

ASP.NET Core Web API for managing and serving content for my personal developer portfolio.

This project is built as a professional full-stack backend service using a lightweight Clean Architecture approach. The goal is to showcase backend development, API design, SQL Server integration, Docker-based local development, testing, and maintainable project structure.

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

## Architecture Overview

This project follows a lightweight Clean Architecture / layered architecture structure.

```text
Rusiru.Portfolio.Api
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
├── Dockerfile
├── docker-compose.yml
├── .dockerignore
├── .gitignore
├── .env.example
└── README.md
```

---

## Project Layers

### `Rusiru.Portfolio.Api`

The API entry point.

Responsible for:

* Controllers
* API endpoint definitions
* Middleware
* Authentication and authorization setup
* Swagger/OpenAPI configuration
* Dependency injection setup
* HTTP request/response handling

---

### `Rusiru.Portfolio.Application`

The application/use-case layer.

Responsible for:

* DTOs
* Interfaces
* Application services
* Validation logic
* Business use cases
* Request/response models

---

### `Rusiru.Portfolio.Domain`

The core domain layer.

Responsible for:

* Domain entities
* Enums
* Domain rules
* Core business models

---

### `Rusiru.Portfolio.Infrastructure`

The infrastructure layer.

Responsible for:

* Entity Framework Core
* SQL Server database access
* DbContext
* Database migrations
* External services
* Repository implementations, if required

---

### `Rusiru.Portfolio.UnitTests`

The unit test project.

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

Swagger should be available at:

```text
http://localhost:5000/swagger
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

Or run a specific test project:

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

## Suggested API Areas

Planned API areas:

```text
/api/professional-summary
/api/projects
/api/skills
/api/experience
/api/certifications
/api/recommendations
/api/get-in-touch
/api/admin
```

Public endpoints will serve portfolio content.

Admin endpoints will be protected and used to manage portfolio data.

---

## Functional Requirements

* Users can view the hero section content.
* Users can view the About Me / Professional Summary section.
* Users can view the Technical Skills section.
* Users can view the Experience section.
* Users can view the Projects section.
* Users can view mentor or LinkedIn recommendation highlights.
* Users can view the Certifications section.
* Users can use the Get in Touch / Contact section.
* Users can view footer/social information.
* Admin users can manage portfolio content.

---

## Non-Functional Requirements

* Availability: The application should maintain high availability, ensuring API endpoints remain accessible to users with minimal service interruption.
* Security: API endpoints must be secured, especially admin-only endpoints.
* Maintainability: The application should follow a clean, modular, and maintainable design.
* Performance: Performance: API endpoints should return portfolio content quickly and efficiently, with minimal response delay under normal usage.

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

Rusiru Dilshan

Software Engineer

---

## License

This project is for personal portfolio and learning purposes.
