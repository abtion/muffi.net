# MuffiNet

1. [Requirements](#requirements)
2. [Developing](#developing)
   - [First time setup](#first-time-setup)
     - [1. Configuration](#1-configuration)
       - [Database connection](#database-connection)
     - [2. Dependencies and database setup](#2-dependencies-and-database-setup)
     - [3. Ensure that linting and tests pass](#3-ensure-that-linting-and-tests-pass)
   - [Day-to-day](#day-to-day)
3. [Production](#production)
   1. [Deployments](#deployments)
4. [Third party services](#third-party-services)
5. [Backend projects](#backend-projects)
6. [How to use the template](#how-to-use-the-template)

# Requirements

You must have the following installed and available on your machine:

- **Microsoft .NET 5**
- **Microsoft SQL Server Compact running locally on in Docker**
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

# Backend projects

Template to kick start .NET applications at Abtion A/S

The template is built with the Microsoft Azure stack in mind - it can be hosted at other cloud providers. The template uses either a Microsoft Azure SQL Database (Relational DB) or Microsoft Azure Cosmos DB (Document DB).

The template can be developed and debugged on all platforms supported by Microsoft .NET (Windows, Linux, and macOS).

## Projects

### Muffi.Backend

Contains handlers (services) and the domain model (ORM etc)
For seperation of queries (fast) and commands (slower) the pattern Command and Query Responsibility Segregation (CQRS) is used.

### Muffi.FrontendReact

Contains API controllers and the frontend applications in the folder "ClientApp". The frontend uses Jest for tests and the tests are located along side the React components.

## Test projects

### Muffi.Backend.Tests

Contains backend unit tests of handlers.

### Muffi.Frontend.Tests

Contains backend unit tests of controllers and SignalR hubs.

### Muffi.Selenium.Tests

Contains end-to-end tests running in a headless browser (Selenium).

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

# How to use the template
- Create a new project with Muffi.Net as template
   - Set permissions for the Abtion user to Read
   - Set permissions for the project specific user to Admin
- Rename Solution from "MuffiNet" to [ProjectName]
- Rename Projects from "MuffiNet" to [ProjectName]
- Find and Replace from “MuffiNet” to [ProjectName]
- Close Visual Studio/VS Code
- Rename project and test folders from File Explorer or similar
- Replace MuffiNet to [ProjectName] in .sln file
- Replace MuffiNet to [ProjectName] in .yarnrc file
- Open Visual Studio/VS Code
- Run yarn install
- Rebuild Solution
- Create Azure App Service
   - An instance of Application Insights is created automatically
- Change the connected service "Azure Application Insights" in the "FrontendReact" project
   - Manage Connected Services -> Restore on Application Insights
   - Replace Application Insights ConnectionString in both Backend and FrontendReact projects
- Create a new Azure SQL Database
   - Configure the database to be serverless and with a sleep timer of 1 hour and the maximum of 2 Gb of storage to save costs
   - Maybe there it is needed to setup access from certain IP-numbers in the firewall of GitHub Actions
   - Change the connection string in AppSetting.json to an appropriate database name (appSettings.Test.json should use a different database name than the other files)
   - Save the connection string in GitHub Actions Secrets "MSSQL_CONNECTION_STRING"
- Update the publish profile 
- Setup pipeline to deploy application
   - In the Azure Portal go to App Service and choose "Deployment Center"
      - Select GitHub as source
      - Click "Authorize" and finish wizard (maybe log into GitHub with project-specific-user-name: project@abtion.com?)
      - Connect the GitHub organisation, project and branch to the deployment slot (production)
      - Save changes and download the publish profile (Manage Publish Profiles)
      - Save the publish profile in GitHub Action Secrets "AzurePublishProfile" with the content of the downloaded profile.publishsettings
   - In CI pipeline-file
      - Remove " && 'to-enable-azure-deploy' == 'remove-this-after-configuring-github-secrets-and-below-settings'"
      - Replace "MuffiNet" with [ProjectName] in database migration step "dotnet ef database update --project src/MuffiNet.FrontendReact"
      - Change the publish-profile line to publish-profile: ${{ secrets.AzurePublishProfile }}
      - Change the name of the Azure App Service in app-name

   
