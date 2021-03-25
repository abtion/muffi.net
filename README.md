# muffi.net
Template to kick start .NET applications at Abtion A/S

The template is built with the Microsoft Azure stack in mind - it can be hosted at other cloud providers. The template uses either a Microsoft Azure SQL Database (Relational DB) or Microsoft Azure Cosmos DB (Document DB).

The template can be developed and debugged on all platforms supported by Microsoft .NET (Windows, Linux, and macOS).

## Projects

### Abtion.Muffi.DomainModel
The domain model is where single object manipulation is done - Create, Read, Update, Delete (CRUD).

For seperation of queries (fast) and commands (slower) the pattern Command and Query Responsibility Segregation (CQRS) is used.

### Abtion.Muffi.Services
The service layer is where operations across multiple objects is done (business logic).

## Test projects

## Nuget Packages Used
### Microsoft Entity Framework Core
- https://docs.microsoft.com/en-us/ef/core/

### Microsoft Azure Cosmos DB
- https://docs.microsoft.com/en-us/azure/cosmos-db/introduction

### Microsoft Azure SQL Database
- https://docs.microsoft.com/en-us/azure/azure-sql/database/sql-database-paas-overview

## Design Patterns Used 
### CQRS further reading
- https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs
- https://www.c-sharpcorner.com/article/using-the-cqrs-pattern-in-c-sharp/

### Dependency Injection further reading
- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0
