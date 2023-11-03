# ArchZ
Sample implementation of an enterprise application architecture with a collection of principles and strategies for good practices

## How it works

- DDD
- CQRS
- Events
- Observability
- Authentication and authorization
- Fail fast validations
- EF Core Code First
- Integration and Unit Tests

### To Do
- [ ] Functions
	- [ ] Login
	- [ ] Register
	- [ ] Create order
	- [ ] List order (with filters)
- [ ] Seed database
- [ ] Observability
	- [ ] Logs with Serilog
	- [ ] APM tool
- [ ] Authentication and authorization
	- [ ] OpenIddict
	- [ ] JWT Token
	- [ ] MFA: OTP
- [ ] CQRS
	- [ ] Mediator
	- [ ] Caching/Read database
- [ ] Events/async processing
	- [ ] MassTransit 
- [ ] Tests
	- [ ] Unit
	- [ ] Integration
- [ ] Correlate logs between microservices http requests
- [ ] Add user secrets

## Getting Started

Run on base project folder to update database
```
dotnet ef database update
```

To add a new migration
```
dotnet ef migrations add <name> --context Arch.Auth.Api.Infra.ApplicationDbContext
```

Run services and databases
```
docker-compose up -d
docker-compose ps
```
