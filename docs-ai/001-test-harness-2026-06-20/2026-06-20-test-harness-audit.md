# Test Harness Audit

## Risks Checked

- Products startup currently requires database and JWT settings even for test hosts. Test factories must provide in-memory configuration.
- `AppReadDbContext` and `AppWriteDbContext` represent separate types. InMemory persistence does not prove SQL Server migrations or relational behavior.
- Products read tests should seed read models directly because the repository writes aggregate entities and the read model is a separate CLR type.
- Architecture tests must reflect the architecture implemented today, including Products read queries depending on `AppReadDbContext`.
- Message queue tests cannot be implemented honestly until queue/message infrastructure exists.

## Harness Follow-Ups

- Add a reusable `Archz.Testing` building block if a second service needs the same `WebApplicationFactory` customization.
- Add SQL Server container integration tests for EF migrations, precision, identity generation, and provider-specific behavior.
- Add Users Identity behavioral/integration tests after the Products harness lands.
- Add a future messaging test fixture once MassTransit/outbox contracts are implemented.
- Consider an `architecture-testing` skill or harness agent focused on preserving service boundaries and test-layer taxonomy.

## Final Harness Analysis

What improved:

- The solution now has a repeatable test taxonomy instead of only planned tests.
- Products behavioral tests enter through the real HTTP route and replace only `IProductRepository` and `IUnitOfWork`.
- Products integration tests exercise the real ASP.NET Core, MediatR, validation, handler, repository, unit-of-work, and EF Core path with fast in-process persistence.
- Architecture tests now protect implemented service boundaries and building-block direction.
- Docs now describe the implemented test strategy, local commands, and pending queue/provider-specific coverage.

What should improve next:

- Add `tests/Services/Users/Archz.Users.Tests` for signin/signup behavioral scenarios. This likely needs a small Identity test fixture or fakes around `UserManager<User>` and `ITokenService`.
- Add a reusable test fixture library only after Users needs the same host/configuration helpers. A premature shared testing building block would be mostly ceremony today.
- Add a `scripts/test.ps1` or `build.ps1` wrapper if the team wants one command for restore, build, targeted tests, full tests, and optional coverage.
- Add CI once the workflow policy is chosen. A first workflow can run `dotnet test Archz.sln` without Docker because current tests are local-only.
- Add SQL Server container tests before relying on migrations, SQL translation, precision, concurrency, or relational constraints.
- Add message-consumer behavioral tests only after message contracts and MassTransit/outbox infrastructure exist.

Potential new agent:

- `test-harness-designer`: Given a service feature, it would classify scenarios into behavioral, integration, architecture, and provider-specific tests; identify mockable boundaries; and produce a fixture plan before implementation.

Potential new skill:

- `dotnet-test-harness`: A repo-local testing skill for .NET services covering `WebApplicationFactory`, EF Core provider selection, Identity fakes, message-consumer tests, architecture rules, and doc updates.

Potential missing docs:

- A dedicated `docs/TESTING.md` would be useful once Users and messaging tests land. For now, `docs/QUALITY.md` and `docs/SERVICE-PATTERNS.md` are sufficient and avoid fragmenting the source of truth.
