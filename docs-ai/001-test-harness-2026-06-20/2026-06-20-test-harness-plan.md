# Test Harness Plan

## Goal

Add the first committed test harness for Archz with three useful layers:

- Behavioral tests that enter through HTTP controllers and replace only boundary dependencies such as repositories and unit of work.
- Integration tests that enter through HTTP controllers and exercise ASP.NET Core, MediatR, validators, handlers, repositories, and EF Core using an in-process test database.
- Architectural tests that protect service boundaries, controller conventions, and domain-layer independence.

## Selected Scope

Start with Products because it has the clearest implemented command/query flow:

- `ProductsController`
- `CreateProductCommand`
- `CreateProductCommandHandler`
- `CreateProductCommandValidator`
- `ProductService`
- `IProductRepository`
- `ProductRepository`
- `AppWriteDbContext`
- `AppReadDbContext`

Add solution-level architecture tests across Users, Products, Orders, Application.Core, and SharedKernel.

## Test Strategy

Behavioral tests:

- Use `WebApplicationFactory<Program>` so the entry point is the real controller route.
- Keep MediatR, validators, handlers, and domain service real.
- Replace `IProductRepository` and `IUnitOfWork` with in-memory fakes.
- Cover valid product creation, duplicate-name business failure, and request validation failure.

Integration tests:

- Use `WebApplicationFactory<Program>`.
- Replace Products SQL Server DbContexts with EF Core InMemory contexts.
- Keep the real repository and unit of work.
- Cover product creation and product reads.

Architectural tests:

- Use reflection-based checks instead of adding a specialized architecture testing package.
- Verify controllers use API controller and versioned route conventions.
- Verify service projects do not reference each other.
- Verify domain types do not depend on service application, infrastructure, controllers, or startup namespaces.
- Verify command and query handler naming/namespace conventions.

## Out Of Scope

- Message queue behavioral tests, because async external messaging is still planned and no MassTransit/outbox contracts exist yet.
- Users integration tests, because Identity scenarios need a slightly richer fixture and are better added after the initial harness shape is proven.
- SQL Server/Testcontainers-based persistence tests. The first integration layer should be fast and local; relational-container tests can be added later for migrations/provider behavior.

## Sources Consulted

- `AGENTS.md`
- `README.md`
- `docs/README.md`
- `docs/ARCHITECTURE.md`
- `docs/SERVICE-PATTERNS.md`
- `docs/DATA-ACCESS.md`
- `docs/QUALITY.md`
- `src/Services/Products/Archz.Products.Api/Controllers/ProductsController.cs`
- `src/Services/Products/Archz.Products.Api/Application/Commands/CreateProduct/*`
- `src/Services/Products/Archz.Products.Api/Domain/AggregateModels/ProductAggregates/*`
- `src/Services/Products/Archz.Products.Api/Infra/*`
