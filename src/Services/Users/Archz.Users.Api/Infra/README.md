## Ef Cli

https://learn.microsoft.com/en-us/ef/core/cli/dotnet

```sh
## List migrations
dotnet ef migrations list

## Update current database
dotnet ef database update

# Removes the last migration, rolling back the code changes that were done for the latest migration.
dotnet ef migrations remove

# Adds a new migration.
dotnet ef migrations add <name> --context Archz.Users.Api.Infra.AppDbContext -o Infra/Migrations

# Generates a SQL script from migrations.
dotnet ef migrations script

# Creates an executable to update the database.
dotnet ef migrations bundle
```
