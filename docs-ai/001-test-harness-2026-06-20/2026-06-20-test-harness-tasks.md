# Test Harness Tasks

## Implementation Tasks

- Add `tests/Services/Products/Archz.Products.Tests` as an xUnit test project.
- Add a behavioral `WebApplicationFactory` that overrides `IProductRepository` and `IUnitOfWork` with test fakes.
- Add Products behavioral HTTP scenarios for valid create, duplicate-name failure, and validation failure.
- Add an integration `WebApplicationFactory` that swaps Products DbContexts to EF Core InMemory.
- Add Products integration HTTP scenarios for create, list, and get-by-id behavior.
- Add `tests/Architecture/Archz.Architecture.Tests` as an xUnit test project.
- Add reflection-based architecture rules for service references, controllers, domain dependencies, and handler conventions.
- Add both projects to `Archz.sln`.
- Update `docs/QUALITY.md`, `docs/LOCAL-DEVELOPMENT.md`, `docs/ARCHITECTURE.md`, `docs/SERVICE-PATTERNS.md`, and `docs/DECISIONS.md`.
- Run `dotnet test Archz.sln` and `dotnet build Archz.sln`.

## Acceptance Criteria

- Tests can be run from the repository root.
- Behavioral tests enter through real HTTP routes.
- Behavioral tests mock only repository/unit-of-work boundaries.
- Integration tests use real Products application flow and EF Core test persistence.
- Architecture tests cover current service/building-block boundaries without blocking documented existing patterns.
- Docs describe implemented test behavior and label pending queue/SQL Server-container coverage.
