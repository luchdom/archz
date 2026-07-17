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

### Test Harness Taxonomy

Tests use xUnit. Behavioral tests should enter through a real adapter such as an HTTP controller or future message consumer and replace only boundary dependencies such as repositories, units of work, message buses, and external providers. Products is the first implemented example.

Integration tests should enter through HTTP routes when testing API services. The current Products integration tests use EF Core InMemory for fast local execution and keep the real repository/unit-of-work flow. SQL Server container tests are reserved for provider-specific behavior.

Architecture tests use reflection and protect implemented rules only. Planned architecture should be documented as pending instead of encoded as a failing rule.

## Pending Decisions

### Domain Event Dispatch

Products dispatches domain events in-process from EF Core save changes through MediatR notifications. Decide whether cross-service events should use an outbox, a message broker, or both.

### Async Messaging

The README names MassTransit as a planned item. Decide broker, message contracts, ownership, retry behavior, and whether an outbox pattern is required.

### Read Model Strategy

Products has an `AppReadDbContext` and read model for product list and lookup queries. Decide whether future reads continue using the same SQL database or move to projections, cache, or a separate read database.

### Observability

The README lists Serilog, APM, and cross-service request correlation as planned. Decide logging library, correlation ID propagation, tracing backend, metrics, and local development defaults.

### Orders Service Scope

Orders exists as a scaffolded API. Decide the first aggregate boundaries, commands, queries, and service dependencies before implementation.

### Test CI Requirements

Decide the GitHub Actions workflow and whether all tests or only fast local tests should run on every pull request.

### Secrets And Configuration

Development appsettings currently include local/demo SQL and JWT values. Decide the policy for user secrets, environment variables, and production configuration examples.
