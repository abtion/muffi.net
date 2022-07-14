# MuffiNet

1. [Requirements](#requirements)
2. [Developing](#developing)
   - [First time setup](#first-time-setup)
     - [1. Configuration](#1-configuration)
       - [Database connection](#database-connection)
       - [Azure Active Directory](#azure-active-directory)
     - [2. Dependencies and database setup](#2-dependencies-and-database-setup)
     - [3. Ensure that linting and tests pass](#3-ensure-that-linting-and-tests-pass)
   - [Day-to-day](#day-to-day)
   - [Test Coverage](#test-coverage)
3. [Configure Code Climate](#configure-code-climate)
4. [Production](#production)
   1. [Deployments](#deployments)
5. [Third party services](#third-party-services)
6. [Backend projects](#backend-projects)
7. [How to use the template](#how-to-use-the-template)

# Requirements

You must have the following installed and available on your machine:

- **Microsoft .NET 5**
- **Microsoft SQL Server Compact running locally on in Docker**
- **Node JS >12.x** (16.11.0)
- **Yarn 1.x**
- https://github.com/coreybutler/nvm-windows


# Developing

## First time setup

### 1. Configuration

Local overrides of configurations/secret must be done through [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)

These projects have user secrets:
  - MuffiNet.Selenium.Tests
  - MuffiNet.Api
  - MuffiNet.FrontendReact



#### Database connection

The project comes with connection strings for a docker container specified in `docker-compose.yml`.
Start the DB with `docker-compose up`, then no further setup is required.

##### LocalDB

If you are running Windows with a instance of MS LocalDb, you can avoid using Docker for development and for the Selenium tests by running these commands.

In the "MuffiNet.Selenium.Tests" project folder:
`dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\mssqllocaldb;Database=MUFFINET-TEST;Trusted_Connection=True;MultipleActiveResultSets=true"`

In the "MuffiNet.FrontendReact" project folder:
`dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\mssqllocaldb;Database=MUFFINET;Trusted_Connection=True;MultipleActiveResultSets=true"`

In the "MuffiNet.Api" project folder:
`dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\mssqllocaldb;Database=MUFFINET;Trusted_Connection=True;MultipleActiveResultSets=true"`

#### Azure Active Directory

In the Azure Portal, [create an "Enterprise Application"](https://docs.microsoft.com/en-us/azure/active-directory/manage-apps/add-application-portal) in Active Directory.

In the following two sections, we will walk through the configuration steps, and collect
the IDs required for the `ActiveDirectoryConfig` properties, which are configured via the
`appsettings.json` file - this is done from two different areas in the Active Directory
management tool in Azure Portal.

##### From the "App Registrations" area, select your application.

Under "App Roles", create a new "App Role" with allowed member types "Users / Groups",
and a "Value" of `Administrators`.

Take down the ID of this Role - this is your `AdminRoleID` for your `ActiveDirectoryConfig`.

Note that:

  - Currently there is no UI to copy the Administrator Role ID - if the ID isn't fully
    visible, use DevTools to inspect the ID value and copy it from the `title` attribute
    on that element.

  - Users with this Role will have the right to assign Roles to other Users via `/admin`.

  - Roles cannot be assigned from the Azure Portal without a Premium subscription.
    (Creating the first Administrator User will be covered below.)

Under "API Permissions", add a new Permission for the "Microsoft Graph" API - select the
"Application Permissions" type, and select the following Permissions:

* `Application.Read.All`
* `AppRoleAssignment.ReadWrite.All`

Press "Add Permissions", and then click the "Grant admin consent" button to confirm.

From the "Overview" tab, take down the following details for your `ActiveDirectoryConfig`:

* `AppClientID` from "Application (client) ID".
* `AppRegistrationID` from "Object ID"
* `DirectoryTenantID` from "Directory (tenant) ID"

Under "Certificates & Secrets", create a "Client Secret", and take down the "Value" (not
the "Secret ID") - this is your `AppClientSecret` for your `ActiveDirectoryConfig`.

⚠ **The secret will be displayed only once** - it should be recorded in a password manager.

TODO the secret should be stored in Secret Manager, rather than appsettings.json ??

##### From the "Enterprise Applications" area, select your application.

From the "Overview" tab, take down the "Object ID" - this is your `AppID` for your
`ActiveDirectoryConfig`.

From the "Users and Groups" screen, create or select an existing User, and take down the
"Object ID" for that User - this is your `AdminUserID` for your `ActiveDirectoryConfig`,
which will be used (at application launch) to assign the `Administrator` Role to
the first User.

(You can safely remove this value from your configuration after the first launch, as from
here on out, this User will be able to grant the `Administrator` Role to other Users in the
Directory via the user interface in `/admin`.)

##### Creating Authorized Controllers

Controllers by default are open and can be accessed by anyone, without logging in.

To limit access to controllers, apply the [Authorize](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authorization.authorizeattribute?view=aspnetcore-6.0)
attribute to your controller class declarations - refer to [this section of the ASP.NET documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-6.0#adding-role-checks)
for details.

Note that the Role name(s) you specify with this attribute must match the "Value" property
of the Role in question - do not confuse this with the "Display Name", which is just a label.

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

## Test Naming Convention

### Selenium Tests (end-to-end)

For these tests we use the Gherkin syntax. Examples:

- Given_SystemIsRunning_When_TheLandingPageIsCall_Then_ThePageIsShown
- Given_UserExists_When_LoggingIn_Then_UserIsLoggedInAndRedirected
- Given_UserDoesNotExist_When_LoggingIn_Then_UserIsNotLoggedIn
- Given_UserExistsAndIsLoggedIn_When_LoggingOut_Then_UserIsLoggedOutAndRedirected

### Unit Tests and Integration Tests

For these tests we use a shorter naming convention: MethodName_ExpectedBehavior_StateUnderTest. Examples:

- LoadAll_ReturnTypeIsCorrect_WhenRequestIsValid
- GetContactFormInformationIsCalled_ReturnsLoadContactFormResponse_WhenRequestIsValid
- LoadJiraIssueDetails_ReturnsAnJiraIssueDetailsInstanceWithCorrectFields_WhenJiraIdentifierIsCorrect

## Test Coverage

Backend code coverage requires Coverlet & ReportGenerator:

- `dotnet tool install -g dotnet-reportgenerator-globaltool`
- `dotnet tool install -g coverlet.console`

Backend report: (note slash/backslash use differs on different cmd shells)

- `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov ./tests/MuffiNet.Backend.Tests`
- `dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov ./tests/MuffiNet.FrontendReact.Test`
- `reportgenerator "-reports:./tests/**/*.info" "-targetdir:coverage"`

Frontend report:

- `yarn test --coverage`

# Configure Code Climate

1. Login to CodeClimate and add your project
2. Go to the `Repo Settings` -> `Test coverage`
3. Enable `Enforce Diff Coverage` with a threshold of 100%
4. Copy the `TEST REPORTER ID`
5. Visit `https://github.com/abtion/<Project name>/settings/secrets`
6. Add a new Github Secret: `CC_TEST_REPORTER_ID` with the copied value from codeclimate

# Production

## Deployments

TBD

# Third party services

# Backend projects

Template to kick start .NET applications at Abtion A/S

The template is built with the Microsoft Azure stack in mind - it can be hosted at other cloud providers. The template uses either a Microsoft Azure SQL Database (Relational DB) or Microsoft Azure Cosmos DB (Document DB).

The template can be developed and debugged on all platforms supported by Microsoft .NET (Windows, Linux, and macOS).

## Projects

### Muffi.API

Contains a Minimal API setup for pure backend API projects.

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

- https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0

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
- Replace the User Secret Keys in these projects (by deleting the existing keys in the project files and running `dotnet user-secrets init` in each of the directories):
  - MuffiNet.Selenium.Tests
  - MuffiNet.Api
  - MuffiNet.FrontendReact
- Open Visual Studio/VS Code
- `yarn install`
- `dotnet build`
- Provision Azure resources through script 'azure-setup.ps1'
  - Log in to portal.azure.com as [ProjectName]@abtion.com
  - Open cloud shell (Powershell)
  - Fill in azure-setup.ps1 variables at top of script (complex auto-generated password recommended)
  - Paste script in cloud shell to provision Azure resources
- Application Insights:
  - (Visual Studio only) Manage Connected Services -> Restore on Application Insights
  - Replace Application Insights ConnectionString in both Backend and FrontendReact appsettings.\*.json files
- SQL:
  - Potentially, access from certain IP-numbers in the firewall of GitHub Actions is needed (in some cases "52.170.187.191")
    - Insert these firewall rules through Azure portal yourself
  - Save the SQL database connection string in GitHub Secrets "MSSQL_CONNECTION_STRING"
- Setup pipeline to deploy application:
  - In the Azure Portal go to App Service and choose "Deployment Center"
    - Select GitHub as source
    - Click "Authorize" and finish wizard (maybe log into GitHub with project-specific-user-name: project@abtion.com?)
    - Connect the GitHub organisation, project and branch to the deployment slot (production)
    - Save changes and download the publish profile (Manage Publish Profiles)
    - Save the publish profile in GitHub Secrets "AzurePublishProfile" with the content of the downloaded profile.publishsettings
  - In CI pipeline-file
    - Remove " && 'to-enable-azure-deploy' == 'remove-this-after-configuring-github-secrets-and-below-settings'"
    - Replace "MuffiNet" with [ProjectName] in database migration step "dotnet ef database update --project src/MuffiNet.FrontendReact"
    - Replace "MuffiNet" with [ProjectName] twice in publish step "run: dotnet publish src/MuffiNet.FrontendReact/MuffiNet.FrontendReact.csproj -c Release -o ${{env.DOTNET_ROOT}}/myapp"
    - Change the publish-profile line to publish-profile: ${{ secrets.AzurePublishProfile }}
    - Change the name of the Azure App Service in app-name
