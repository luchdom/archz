# Local Development

## Prerequisites

- .NET 8 SDK.
- Docker Desktop or another Docker Compose-compatible runtime.
- Optional: EF Core CLI tools for migration work.

## Build

From the repository root:

```sh
dotnet build Archz.sln
```

The solution currently builds successfully, but it emits warnings. See `docs/QUALITY.md` for the current validation baseline.

## Run With Docker Compose

From the repository root:

```sh
docker-compose up -d
docker-compose ps
```

Defined services:

- `archz.users.api`
- `sql-server-users`
- `archz.products.api`
- `sql-server-products`

Default host ports from `docker-compose.override.yml`:

- Users API HTTP: `8080`
- Users API HTTPS: `8081`
- Products API HTTP: `7080`
- Products API HTTPS: `7081`
- Users SQL Server: `1433`
- Products SQL Server: `1434`

Swagger is enabled in Development and Staging.

## Configuration

Development appsettings include local database connection strings and JWT values. Treat them as local/demo values only. Do not reuse them for production or shared environments.

## Database Migrations

Users has an EF CLI note at `src/Services/Users/Archz.Users.Api/Infra/README.md`.

Common commands:

```sh
dotnet ef migrations list
dotnet ef database update
dotnet ef migrations remove
dotnet ef migrations add <name> --context <fully-qualified-context> -o Infra/Migrations
dotnet ef migrations script
dotnet ef migrations bundle
```

Use the relevant context:

- Users: `Archz.Users.Api.Infra.AppDbContext`
- Products: `Archz.Products.Api.Infra.AppWriteDbContext`

## Current Local Gaps

- Orders is not wired into `docker-compose.yml`.
- There are no committed test projects.
- There are no committed GitHub Actions workflow files.
