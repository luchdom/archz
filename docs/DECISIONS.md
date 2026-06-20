# Decisions

This file records architecture decisions that are visible in the current repository, plus unresolved decisions that should be made before expanding the architecture further.

## Accepted Decisions

### Service-Oriented API Projects

Users, Products, and Orders are separate ASP.NET Core API projects under `src/Services`. Services should own their own application, domain, infrastructure, startup, settings, and controller code.

### Shared Building Blocks

Common ASP.NET Core and application behavior belongs in `Archz.Application.Core`. DDD seedwork and cross-service domain primitives belong in `Archz.SharedKernel`.

### Thin Controllers With MediatR

Controllers delegate application flow to MediatR commands and queries. Validation and logging are handled by MediatR pipeline behaviors registered from `AddCore<T>()`.

### Result-Based Expected Failures

The repo uses `FluentResults` for expected validation and business failures. Controllers map failures to problem responses through `BaseController`.

### EF Core And SQL Server

Services use EF Core with SQL Server. Users uses ASP.NET Core Identity storage. Products uses a write/read DbContext split as the initial CQRS persistence direction.

### Docker Compose For Local Dependencies

Local development uses Docker Compose to run API services and SQL Server containers for Users and Products.

## Pending Decisions

### Domain Event Dispatch

Entities can collect domain events, but dispatch is not implemented. Decide whether dispatch should happen inside EF Core save changes, through MediatR notifications, through an outbox, or through a message broker.

### Async Messaging

The README names MassTransit as a planned item. Decide broker, message contracts, ownership, retry behavior, and whether an outbox pattern is required.

### Read Model Strategy

Products has an `AppReadDbContext`, but read models and read configurations are not implemented. Decide whether reads use the same SQL database, projections, cache, or a separate read database.

### Observability

The README lists Serilog, APM, and cross-service request correlation as planned. Decide logging library, correlation ID propagation, tracing backend, metrics, and local development defaults.

### Testing Strategy

No test projects are committed. Decide test framework, naming, fixture style, integration database strategy, and CI requirements.

### Orders Service Scope

Orders exists as a scaffolded API. Decide the first aggregate boundaries, commands, queries, and service dependencies before implementation.

### Secrets And Configuration

Development appsettings currently include local/demo SQL and JWT values. Decide the policy for user secrets, environment variables, and production configuration examples.
