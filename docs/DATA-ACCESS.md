# Data Access

Archz uses EF Core with SQL Server. Users and Products each own their own database connection in development configuration and Docker Compose.

## Current Databases

Docker Compose defines:

- `sql-server-users`, exposed on host port `1433`.
- `sql-server-products`, exposed on host port `1434`.

The API containers connect to those database services through service names in `appsettings.Development.json`.

## Users Persistence

Users uses ASP.NET Core Identity with a custom `AppDbContext` that derives from `IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>`.

The model maps Identity tables to explicit snake-case-style table names:

- `users`
- `roles`
- `user_roles`
- `user_claims`
- `user_logins`
- `user_tokens`
- `role_claims`

Users migrations live under `src/Services/Users/Archz.Users.Api/Infra/Migrations`.

## Products Persistence

Products uses an explicit write/read DbContext split:

- `AppWriteDbContext`: tracks aggregates and implements `IUnitOfWork`.
- `AppReadDbContext`: configured as no-tracking and intended for read-side models.

The write context applies configurations from `Infra/EntityConfigurations/Write`. The read context applies configurations from `Infra/EntityConfigurations/Read`.

`ProductRepository` writes through `AppWriteDbContext`. Product read queries use `AppReadDbContext` with `ProductReadModel`.

## Migrations

Existing migrations are committed under each service's `Infra/Migrations` folder.

Useful EF CLI commands:

```sh
dotnet ef migrations list
dotnet ef database update
dotnet ef migrations remove
dotnet ef migrations add <name> --context <fully-qualified-context> -o Infra/Migrations
dotnet ef migrations script
dotnet ef migrations bundle
```

Example context names:

- `Archz.Users.Api.Infra.AppDbContext`
- `Archz.Products.Api.Infra.AppWriteDbContext`

Run EF commands from the relevant service project folder or pass `--project` and `--startup-project` explicitly.

## Unit Of Work

`IUnitOfWork` is defined in `Archz.SharedKernel`. Products registers it by resolving the write DbContext. Command handlers should call `SaveChangesAsync` after successful aggregate changes.

## Domain Events

The shared `Entity` base class can collect domain events. Products adds `ProductCreatedEvent` when a product is created.

Products dispatches collected domain events through MediatR after `AppWriteDbContext.SaveChangesAsync` succeeds, then clears the events from tracked entities.

External publication is still pending. Do not document or rely on domain events being published to queues or external services until an outbox or message broker integration is implemented.

## Auditing

`IAuditable` defines `CreatedOnUtc` and `ModifiedOnUtc`. Products has `UpdateAuditableInterceptor`, which sets those values on tracked auditable entities during save.

This is only effective for entities that implement `IAuditable` and are tracked by a DbContext with the interceptor registered.
