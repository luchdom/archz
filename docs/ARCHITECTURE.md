# Architecture

Archz is a .NET 8 sample enterprise architecture solution. It demonstrates service-oriented API boundaries with DDD, CQRS-style application slices, EF Core persistence, JWT authentication, and shared building blocks.

The root `README.md` lists a broader target architecture. Some items are implemented, some are partial, and some remain planned.

## Solution Layout

- `Archz.sln`: Visual Studio solution containing services, building blocks, and Docker Compose project entries.
- `src/Services/Users/Archz.Users.Api`: user registration, login, JWT token creation, and ASP.NET Core Identity persistence.
- `src/Services/Products/Archz.Products.Api`: product aggregate, create product command, repository, EF Core write/read DbContext structure, and JWT validation.
- `src/Services/Orders/Archz.Orders.Api`: scaffolded API service with placeholder weather endpoint and folders for future application, domain, infrastructure, and settings code.
- `src/BuildingBlocks/Archz.Application.Core`: shared ASP.NET Core and application-layer infrastructure.
- `src/BuildingBlocks/Archz.SharedKernel`: shared DDD seedwork and result abstractions.

## Service Boundaries

Each service is an ASP.NET Core API project. Users and Products currently reference the shared building blocks. Orders is present in the solution but is still mostly scaffolded.

Services should own their own application, domain, infrastructure, settings, startup, and controller code. Avoid direct coupling between service projects. Shared code belongs in a building block only when it is reusable across services and does not encode service-specific business behavior.

## Shared Building Blocks

`Archz.Application.Core` currently provides:

- `AddCore<T>()` startup extension for controllers, JSON enum conversion, routing, Swagger, API versioning, validators, MediatR, validation behavior, and logging behavior.
- `BaseController` helper for converting failed `FluentResults` into problem responses.
- MediatR pipeline behaviors for logging and validation.
- Common extensions such as date/time conversion helpers.

`Archz.SharedKernel` currently provides:

- `Entity` base class with domain event collection.
- `IDomainEvent`.
- `IUnitOfWork`.
- `IAuditable`.
- `ValueObject` and `Enumeration` seedwork.
- Result helper types.

## Application Flow

The implemented service flow is:

1. Controller receives an HTTP request.
2. Controller sends a MediatR command or query.
3. FluentValidation validators run through `ValidationBehavior`.
4. Logging runs through `LoggingBehavior`.
5. Handler coordinates domain services, repositories, and unit of work.
6. Handler returns data or `FluentResults` failures.
7. Controller maps failures through `BaseController.ValidationProblem`.

Products has the clearest implemented command flow through `CreateProductCommand`, `CreateProductCommandValidator`, `CreateProductCommandHandler`, `ProductService`, and `ProductRepository`.

## Current Implementation Status

Implemented:

- .NET 8 ASP.NET Core API services.
- Swagger and API versioning setup through shared application core.
- MediatR registration and pipeline behaviors.
- FluentValidation request validation.
- Users registration and login commands.
- JWT creation in Users and JWT validation in Users and Products.
- ASP.NET Core Identity persistence for Users.
- Product aggregate, repository, EF Core write context, and create product command.
- Docker Compose for Users, Products, and separate SQL Server containers.

Partially implemented:

- DDD aggregate and domain event pattern. Entities can collect domain events, but dispatch is not implemented.
- CQRS. Commands are present and Products has separate read/write DbContext types, but read queries are not implemented.
- Auditing. `IAuditable` and an EF interceptor exist, but coverage depends on entities implementing the interface.
- Orders service structure. The folders exist, but the service remains scaffolded.

Planned or pending:

- Integration and unit tests.
- Observability with structured logs, APM, and cross-service request correlation.
- Async event processing, including MassTransit.
- Read database, cache, or projection layer.
- Complete product CRUD and order features.

## Runtime Topology

Docker Compose defines:

- `archz.users.api`
- `sql-server-users`
- `archz.products.api`
- `sql-server-products`

Users and Products use separate SQL Server containers and connection strings in development appsettings. Orders is in the solution and has a Dockerfile, but it is not currently included as a service in `docker-compose.yml`.
