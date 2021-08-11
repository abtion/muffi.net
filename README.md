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
   - [Twilio](#twilio)
   - [Azure](#azure)

Care1-VCI is a tool that lets technicians have video calls with customers.
The customers wait in a queue, until they are engaged by a technician.

- Asana: https://app.asana.com/0/1200407690083597/board
- Harvest: Care1 -> VCI
- Client: Care1
- Contacts:
  - Product Owner & CEO: Kenneth Holm, kenneth.holm@care1.dk
  - Lead technician: Michael Winther, miha@care1.dk
  - IT Lead: Torben Demant, td@care1.dk

[Twilio](https://www.twilio.com/) is used for implementing the video calls.

# Requirements

You must have the following installed and available on your machine:

- **Dotnet**
- **Dotnet framework 5.0**
- **Docker / LocalDB**
- **Node JS >12.x**
- **Twilio account**
- **Yarn 1.x**

# Developing

## First time setup

### 1. Configuration

If you need to override configuration for the project, you can create a `src/WebAppReact/appsettings.Local.json`-file.

If such a file exists, any values in that file will override the corresponding project settings.

It is also possible to override select environments with `src/WebAppReact/appsettings.[Environment].Local.json`. E.g. `src/WebAppReact/appsettings.Development.Local.json`.

#### Database connection

The project comes with connection strings for a docker container specified in `docker-compose.yml`.
Start the DB with `docker-compose up`, then no further setup is required.

##### LocalDB

If you prefer LocalDB, you can create local configurations for the `Development` and `Test` environments (see [configuration](#1-configuration))

Add the following to `src/WebAppReact/appsettings.Development.Local.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VCI;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  ...
}
```

And to `src/WebAppReact/appsettings.Test.Local.json`:

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
dotnet ef database update --project src/WebAppReact
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

- Run the server: `dotnet run --project src/WebAppReact`
- Run backend tests: `dotnet test`
- Run frontend tests: `yarn test`

# Production

## Script for creating a new user in the database

Use this script:
[sql/create-user.sql](sql/create-user.sql)

As is it will create user with the following credentials:

- care1@abtion.com
- Test1234!

You can change @UserName and @Email to make a user with another email address.

## Deployments

TBD

# Third party services

## Twilio

- **Description:** Twilio is a cloud service that facilitates video/audio/text/etc. conferencing for applications,
- **Auth:** 1password: care1 > twilio.com/login
- **Documentation:** [https://www.twilio.com/docs/video](https://www.twilio.com/docs/video)

## Azure

- **Description:** Azure is Microsoft's cloud hosting service where we host the application.
- **Auth:** 1password: care1 > portal.azure.com
- **Documentation:** [https://portal.azure.com](https://portal.azure.com)
