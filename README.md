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
- [ ] Features
	- [ ] Login
	- [ ] Register
	- [ ] CRUD products
	- [ ] Create order
	- [ ] List order
- [ ] Seed database
- [ ] Observability
	- [ ] Logs with Serilog
	- [ ] APM tool
- [ ] Authentication and authorization
	- [ ] JWT Token
- [ ] CQRS
	- [ ] Mediator
	- [ ] Caching/Read database
- [ ] Events/async processing
	- [ ] MassTransit 
- [ ] Tests
	- [ ] Unit
	- [ ] Integration
- [ ] Correlate logs between microservices http requests

## Getting Started

Run services and databases
```
docker-compose up -d
docker-compose ps
```
