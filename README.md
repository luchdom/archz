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
- Integration, behavioral, and architectural tests

### To Do
- [ ] Features
	- [x] Login
	- [x] Register
	- [ ] CRUD products
	- [ ] Create order
	- [ ] List order
- [ ] Seed database
- [ ] Observability
	- [ ] Logs with Serilog
	- [ ] APM tool
- [x] Authentication and authorization
	- [x] JWT Token 
- [ ] CQRS
	- [ ] Mediator
	- [ ] Caching/Read database
- [ ] Events/async processing
	- [ ] MassTransit 
- [ ] Tests
	- [x] Behavioral/API scenario tests for Products
	- [x] Architectural tests for current service boundaries
	- [x] Integration tests for Products with in-process EF Core persistence
	- [ ] Users authentication tests
	- [ ] Queue/message-driven tests
- [ ] Correlate logs between microservices http requests

## Getting Started

Run services and databases
```
docker-compose up -d
docker-compose ps
```
