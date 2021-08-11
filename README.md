# Care1 VCI

1. [Care1 VCI](#care1-vci)
2. [Requirements](#requirements)
3. [Developing](#developing)
   - [First time setup](#first-time-setup)
     - [1. Configuration](#1-configuration)
       - [Database connection](#database-connection)
     - [2. Dependencies and database setup](#2-dependencies-and-database-setup)
     - [3. Ensure that linting and tests pass](#3-ensure-that-linting-and-tests-pass)
   - [Day-to-day](#day-to-day)
4. [Production](#production)
   1. [Deployments](#deployments)
5. [Third party services](#third-party-services)

# Requirements

You must have the following installed and available on your machine:

- **Dotnet**
- **Dotnet framework 5.0**
- **Docker / LocalDB**
- **Node JS >12.x**
- **Yarn 1.x**

# Developing

## First time setup

### 1. Configuration

If you need to override configuration for the project, you can create a `src/MuffiNet.FrontendReact/appsettings.Local.json`-file.

If such a file exists, any values in that file will override the corresponding project settings.

It is also possible to override select environments with `src/MuffiNet.FrontendReact/appsettings.[Environment].Local.json`. E.g. `src/MuffiNet.FrontendReact/appsettings.Development.Local.json`.

#### Database connection

The project comes with connection strings for a docker container specified in `docker-compose.yml`.
Start the DB with `docker-compose up`, then no further setup is required.

##### LocalDB

If you prefer LocalDB, you can create local configurations for the `Development` and `Test` environments (see [configuration](#1-configuration))

Add the following to `src/MuffiNet.FrontendReact/appsettings.Development.Local.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VCI;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  ...
}
```

And to `src/MuffiNet.FrontendReact/appsettings.Test.Local.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CARE1-VCI-TEST;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  ...
}
```

### 2. Dependencies and database setup

If you don't have yarn, install it:

```sh
npm install -g yarn
```

Restore dotnet project:

```sh
dotnet restore
```

#### Database migrations

First install `dotnet-ef` (if you haven't already):

```sh
dotnet tool install --global dotnet-ef
```

Then run the migrations:

```sh
dotnet ef database update --project src/MuffiNet.FrontendReact
```

You might get an error because the dotnet tools directory is not in your path.

Using zsh or linux/mac, this can be fixed with:

```sh
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.zshrc
source ~/.zshrc
```

Adapting the command to bash/fish etc. should be a matter of replacing `.zshrc`, with for instance `.bashrc`.

### 3. Ensure that linting and tests pass

Backend

```sh
dotnet test
```

Frontend

```sh
yarn test
```

## Day-to-day

- Run the server: `dotnet run --project src/MuffiNet.FrontendReact`
- Run backend tests: `dotnet test`
- Run frontend tests: `yarn test`

# Production

## Script for creating a new user in the database

Use this script:
[sql/create-user.sql](sql/create-user.sql)

As is it will create user with the following credentials:

- user@abtion.com
- Test1234!

You can change @UserName and @Email to make a user with another email address.

## Deployments

TBD

# Third party services

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

### MediatR (Mediator implementation by Jimmy Bogard)

- https://github.com/jbogard/MediatR

## Design Patterns Used

### CQRS further reading

- https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs
- https://www.c-sharpcorner.com/article/using-the-cqrs-pattern-in-c-sharp/
- https://github.com/jbogard/MediatR/wiki

### Dependency Injection further reading

- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0
