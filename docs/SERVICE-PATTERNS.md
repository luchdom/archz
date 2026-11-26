# Service Patterns

This document describes the service structure that current Archz services follow. Products is the richest example. Users demonstrates Identity-specific application flow. Orders is currently scaffolded.

## Project Structure

Service projects generally use these folders:

- `Application`: commands, queries, DTOs, application services, and request handlers.
- `Controllers`: API controllers.
- `Domain`: aggregate models, domain services, domain events, and domain contracts.
- `Infra`: DbContexts, migrations, repositories, seed data, interceptors, and entity configurations.
- `Settings`: strongly typed configuration classes.
- `Startup`: partial static startup extension methods for service-local registration.

## Startup Pattern

`Program.cs` should compose the service from shared core setup plus service-local startup extensions:

```csharp
builder.Services
    .AddCore<Program>()
    .AddCustomSettings(builder.Configuration)
    .AddCustomDbContext(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddCustomHealthChecks(builder.Configuration)
    .AddCustomServices();
```

Keep shared cross-cutting setup in `Archz.Application.Core` when it applies to multiple services. Keep service-specific settings, DbContext, Identity, health check, seed, and dependency registration in that service's `Startup` partial files.

## Controllers

Controllers should stay thin:

- Use `[ApiController]`.
- Use versioned routes like `/v{version:apiVersion}/[controller]`.
- Inject `IMediator`.
- Bind JSON bodies to a top-level record class named `<UseCase>Request` in `Controllers/Requests/<UseCase>Request.cs`.
- Map the HTTP request to the existing application command or query before sending it through MediatR.
- Convert failed `FluentResults` through `BaseController.ValidationProblem`.
- Avoid business rules inside controller actions.

Examples:

- `ProductsController`
- `AuthController`

Keep an existing action parameter name when MVC exposes it in model-binding errors.

Keep one top-level class or record class per handwritten backend C# file. A partial class may span files. Each file still contains one top-level class declaration. The architecture suite checks these rules for the service projects and shared building blocks.

## Commands

Commands live under `Application/Commands/<UseCase>`.

A command folder should usually contain:

- `<UseCase>Command`
- `<UseCase>CommandHandler`
- `<UseCase>CommandValidator` when request validation is needed
- optional response DTOs

Command handlers coordinate domain services, repositories, and unit of work. They should return `FluentResults` when business failures are expected.

## Queries

Queries live under `Application/Queries/<UseCase>`.

The Products service implements list and lookup queries through `AppReadDbContext` and read-side models. Future query implementations should use read-side models or `AppReadDbContext` where appropriate.

Read models should be sealed records with a primary constructor when they only represent query-shaped data and do not need behavior.

## Validation

Request validation uses FluentValidation. Validators are discovered from the service assembly by `AddCore<T>()`.

Validation failures are returned by `ValidationBehavior<TRequest, TResponse>`, which expects result-based responses. Keep validators focused on request shape and simple input rules. Put business invariants in domain services or aggregate methods.

## Domain

Domain code should model business behavior:

- Aggregates should protect their own state.
- Static factory methods can return `Result<T>` for expected creation failures.
- Domain services can coordinate checks that need repositories.
- Domain events can be added to entities through `Entity.AddDomainEvent`.

Products dispatches domain events in-process from `AppWriteDbContext` after a successful save. This is not external messaging; use a future outbox or message broker integration for cross-service events.

## Infrastructure

Infrastructure code contains persistence and external framework concerns:

- EF Core DbContexts.
- Entity type configurations.
- Migrations.
- Repositories.
- Seed data.
- EF interceptors.

Products currently has both `AppWriteDbContext` and `AppReadDbContext`. Users uses ASP.NET Core Identity through a custom `AppDbContext`.

## Settings

Configuration is represented by strongly typed classes under `Settings` and registered by service-local startup code. Development settings include local/demo values only and should not be treated as production secrets.

## Test Patterns

Committed tests use xUnit.

Behavioral service tests should enter through HTTP routes with `WebApplicationFactory<TProgram>`. Keep controllers, MediatR, validators, handlers, and domain services real. Replace only boundaries such as repositories, units of work, message buses, or external providers. The Products behavioral tests demonstrate this by replacing `IProductRepository` and `IUnitOfWork` while keeping `CreateProductCommandHandler` and `ProductService` real.

Integration service tests should also enter through HTTP routes. The current Products integration tests replace SQL Server DbContexts with EF Core InMemory contexts to keep the harness fast and local while exercising the real repository and unit of work. Use provider-specific SQL Server or container tests when the scenario depends on migrations, SQL translation, precision, transactions, identity generation, or relational constraints.

Architecture tests live under `tests/Architecture` and should protect rules that are already implemented. Do not make planned architecture fail the build until the implementation exists.
