# LostDogApp

This is a .NET 8 application for reporting lost dogs, built with ASP.NET Core and Entity Framework Core, using PostgreSQL as the database. The application is containerized with Docker and managed using Docker Compose.

## Prerequisites

- **Docker**: Install Docker Desktop (or Docker CLI on Linux).
- **Docker Compose**: Included with Docker Desktop or install separately.
- **.NET SDK 8.0**: Optional, only needed for local development outside Docker.
- **Git**: To clone or manage the repository.

## Project Structure

```
LostDogApp/
├── LostDogApp/
│   ├── LostDogApp.csproj
│   ├── appsettings.json
│   ├── Models/
│   │   ├── LostDogReport.cs
│   ├── Program.cs
│   └── (other project files)
├── Dockerfile
├── dev.Dockerfile
├── docker-compose.yml
├── docker-compose-migrate.yml
└── README.md
```

- `Dockerfile`: For production builds (uses `mcr.microsoft.com/dotnet/aspnet:8.0`).
- `dev.Dockerfile`: For development and migrations (uses `mcr.microsoft.com/dotnet/sdk:8.0`).
- `docker-compose.yml`: Defines services for development (`web-dev`, `postgres`) and optionally production (`web`).
- `docker-compose-migrate.yml`: Defines services for running database migrations (only `migration`).

## Setup

1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd LostDogApp
   ```

2. **Verify Configuration**:
   Ensure `appsettings.json` has the correct database connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=postgres;Port=5432;Database=lostdogapp;Username=lostdog;Password=password"
     }
   }
   ```
   Check for `appsettings.Development.json` and update it similarly if it exists.

## Running the Application

The application can be run in development mode using `docker-compose.yml`, which starts the `postgres` database and `web-dev` service with hot reloading.

### Steps

1. **Start the Application**:
   ```bash # Build and start services in detached mode docker compose -f docker-compose.yml up -d ```

2. **Access the Application**:
   - Open `http://localhost:8080` in a browser.
   - The `web-dev` service uses `dotnet watch` for hot reloading, so code changes in `LostDogApp/` are reflected automatically.

3. **View Logs**:
   ```bash
   docker compose -f docker-compose.yml logs
   ```

4. **Stop the Application**:
   ```bash
   docker compose -f docker-compose.yml down
   ```

### Notes

- The `postgres` service persists data in the `pgdata` Docker volume.
- If port `8080` or `5432` is in use, edit `docker-compose.yml` to change the host ports (e.g., `8081:8080`).
- For production, uncomment the `web` service in `docker-compose.yml` and run:
  ```bash
  docker compose -f docker-compose.yml up -d web
  ```

## Running Database Migrations

Database migrations are managed using `docker-compose-migrate.yml`, which defines the `migration` service. This allows you to apply Entity Framework Core migrations independently without starting the `web-dev` service. To run `migration` service, make sure that your main `postgres` service (from docker-compose.yml) runs.

### Steps

1. **Build the Migration Service**:
   ```bash
   docker compose -f docker-compose-migrate.yml build
   ```

2. **Run Migrations**:
   ```bash
   docker compose -f docker-compose-migrate.yml up
   ```

3. **Verify Migrations**:
   Check the database schema:
   ```bash
   docker compose -f docker-compose.yml exec postgres psql -U lostdog -d lostdogapp -c "\dt"
   ```
   View migration history:
   ```bash
   docker compose -f docker-compose.yml exec postgres psql -U lostdog -d lostdogapp -c "SELECT * FROM __EFMigrationsHistory;"
   ```
