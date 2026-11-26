# Quality

## Current Validation Baseline

Run this from the repository root:

```sh
dotnet build Archz.sln
dotnet test Archz.sln
```

Current status:

- Build succeeds.
- A clean .NET 8 build reports existing nullable-reference warnings in `Archz.SharedKernel`.
- Committed tests exist for Products behavioral/integration scenarios and solution architecture rules.
- `.github/workflows` exists but no workflow files were found.

Do not claim a broader test layer exists until the relevant test project has been added and run.

## Warning Policy

New work should avoid adding warnings. If warnings are introduced or rediscovered during clean builds, cleanup work should reduce them deliberately and document behavior-affecting changes.

## Empty collection expressions

Use `[]` for empty collections when the target type supports it. The .NET
build and test commands fail with `LUCH0001` when `Array.Empty<T>()` can use
a collection expression in that context. Use an explicit array type when
a call site needs to preserve that type or the selected overload.

## Test Strategy

Committed test projects:

- `tests/Services/Products/Archz.Products.Tests`
  - Behavioral tests enter through Products HTTP routes with `WebApplicationFactory<Program>`.
  - Behavioral tests keep controller, MediatR, validators, handlers, and domain services real.
  - Behavioral tests replace only `IProductRepository` and `IUnitOfWork` with test fakes.
  - Integration tests enter through Products HTTP routes and keep the real repository/unit-of-work flow.
  - Integration tests replace SQL Server with EF Core InMemory persistence for fast local execution.
- `tests/Architecture/Archz.Architecture.Tests`
  - Service projects must not reference each other.
  - Users and Products controllers must use `[ApiController]` and versioned routes.
  - Domain types must not depend on application, infrastructure, controller, startup, or settings namespaces.
  - Command/query handlers must live under matching application namespaces.
  - Building blocks must not reference service assemblies.
  - MVC JSON body request types must be top-level `*Request` record classes in matching C# files under the owning service’s `Controllers/Requests` folder.
  - Each handwritten backend C# file may declare at most one top-level class or record class. The test reads the C# inputs and active symbols from the build, and skips sources with an auto-generated header.

Treat failures in architecture tests as implementation defects. A change that weakens, skips, or removes a rule requires a recorded architecture decision approved by a repository owner and an independent Staff Engineer review. Record the decision in [Decisions](DECISIONS.md) and update the owning guidance and tests together.

Targeted commands:

```sh
dotnet test tests/Services/Products/Archz.Products.Tests/Archz.Products.Tests.csproj
dotnet test tests/Architecture/Archz.Architecture.Tests/Archz.Architecture.Tests.csproj
```

Recommended future test structure:

- Users authentication behavioral and integration tests.
- Domain tests for aggregate invariants and domain services where HTTP entry is not the natural boundary.
- Handler or validation tests only when an HTTP-level scenario would be too slow, too broad, or unclear.
- SQL Server/Testcontainers integration tests for migrations and provider-specific persistence behavior.
- Message-driven behavioral tests after MassTransit/outbox contracts exist.

## Definition Of Done

For code changes:

- Existing build still succeeds.
- Relevant warnings are not increased without explanation.
- New business behavior has focused tests when a test project exists or is added.
- Scenario tests should enter through controllers or message handlers where practical and mock only external boundaries.
- Docs are updated when setup, architecture, data access, service patterns, or validation expectations change.

For docs changes:

- Links and paths match the current repository.
- Planned behavior is clearly labeled as planned or pending.
- Commands are runnable from the documented working directory.
