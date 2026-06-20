# Agent Instructions

These instructions apply to the whole repository.

## First Reads

Before making changes, read:

1. `README.md`
2. `docs/README.md`
3. The nearest service, building block, or infrastructure files for the requested area

When the task affects architecture, setup, validation, workflow, or runtime behavior, update the nearest document under `docs/` in the same change.

## Repository Shape

Archz is a .NET 8 sample enterprise architecture solution. The main source tree is split into:

- `src/Services`: service APIs such as Users, Products, and Orders.
- `src/BuildingBlocks/Archz.Application.Core`: shared ASP.NET Core, Swagger, API versioning, controller, validation, logging, and MediatR setup.
- `src/BuildingBlocks/Archz.SharedKernel`: shared DDD seedwork, domain event abstractions, unit of work contracts, and result helpers.

Prefer service-local implementation unless behavior is clearly shared by multiple services.

## Implementation Rules

- Keep controllers thin. Use MediatR commands and queries for application flow.
- Put write use cases under `Application/Commands/<Feature>` and read use cases under `Application/Queries/<Feature>`.
- Add FluentValidation validators beside commands when request validation is needed.
- Keep business invariants in domain objects or domain services.
- Use `FluentResults` for expected business or validation failures.
- Use EF Core configuration classes under `Infra/EntityConfigurations`.
- Use repositories for aggregate persistence when the service already follows that pattern.
- Do not introduce direct coupling between services.
- Do not move shared behavior into building blocks until at least two services need it or the existing shared pattern already covers it.

## Documentation Rules

- Document implemented behavior as implemented.
- If the README or code expresses intended architecture that is not yet implemented, label it as planned or pending.
- Update `docs/ARCHITECTURE.md` for service boundaries, building blocks, and cross-cutting architecture.
- Update `docs/SERVICE-PATTERNS.md` for controller, command, query, handler, validator, startup, and dependency registration conventions.
- Update `docs/DATA-ACCESS.md` for EF Core, migrations, repositories, DbContexts, Identity storage, or persistence rules.
- Update `docs/LOCAL-DEVELOPMENT.md` for setup, Docker, ports, secrets, or migration commands.
- Update `docs/QUALITY.md` for validation commands, test strategy, CI expectations, or known warning policy.
- Update `docs/DECISIONS.md` for accepted architecture decisions and explicit unresolved tradeoffs.

## Git Naming Rules

- Use task-focused branch names, commit messages, and PR titles.
- Do not include Codex, AI assistant, bot, or tool branding in branch names, commit messages, or PR titles.
- Prefer names such as `docs/add-architecture-docs`, `fix/product-validation`, or `feature/order-creation`.

## Validation

Before finishing code or docs work, run the smallest useful validation. For broad repository changes, run:

```sh
dotnet build Archz.sln
```

At the time these instructions were written, the solution builds successfully but emits warnings and has no committed test projects. Do not claim tests pass unless test projects have been added and run.

## Existing Worktree Changes

The worktree may contain user changes. Do not revert or rewrite unrelated changes. If local changes affect the files you need to edit, read them carefully and preserve the user's intent.
