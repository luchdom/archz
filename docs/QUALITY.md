# Quality

## Current Validation Baseline

Run this from the repository root:

```sh
dotnet build Archz.sln
```

Current status:

- Build succeeds.
- Build emits 4 warnings.
- No committed test projects were found.
- `.github/workflows` exists but no workflow files were found.

Do not claim the repository has passing tests until test projects exist and have been run.

## Warning Policy

The current build warning baseline includes nullability warnings, unread parameters, and possible null dereferences. New work should avoid adding warnings. Cleanup work should reduce the existing warning count deliberately and document behavior-affecting changes.

## Test Direction

Recommended future test structure:

- Unit tests for domain objects and domain services.
- Handler tests for commands and queries.
- Validation tests for FluentValidation rules.
- Integration tests for API endpoints and EF Core persistence.
- Authentication tests for Users login, registration, and JWT behavior.

Suggested future layout:

- `tests/Services/Users/Archz.Users.Tests`
- `tests/Services/Products/Archz.Products.Tests`
- `tests/BuildingBlocks/Archz.SharedKernel.Tests`
- `tests/BuildingBlocks/Archz.Application.Core.Tests`

## Definition Of Done

For code changes:

- Existing build still succeeds.
- Relevant warnings are not increased without explanation.
- New business behavior has focused tests when a test project exists or is added.
- Docs are updated when setup, architecture, data access, service patterns, or validation expectations change.

For docs changes:

- Links and paths match the current repository.
- Planned behavior is clearly labeled as planned or pending.
- Commands are runnable from the documented working directory.
