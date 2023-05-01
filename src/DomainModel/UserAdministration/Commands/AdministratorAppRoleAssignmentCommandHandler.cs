﻿using DomainModel.Shared;
using DomainModel.UserAdministration.Services;
using Microsoft.Graph;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Commands;

public class AdministratorAppRoleAssignmentCommandHandler : ICommandHandler<AdministratorAppRoleAssignmentCommand, AdministratorAppRoleAssignmentResponse>
{
    private readonly IConfiguredGraphServiceClient configuredGraphServiceClient;
    private readonly IAddUserAppRoleAssignmentToAzureIdentity addUserAppRoleAssignmentToAzureIdentity;

    public AdministratorAppRoleAssignmentCommandHandler(
        IConfiguredGraphServiceClient configuredGraphServiceClient,
        IAddUserAppRoleAssignmentToAzureIdentity addUserAppRoleAssignmentToAzureIdentity)
    {
        this.configuredGraphServiceClient = configuredGraphServiceClient;
        this.addUserAppRoleAssignmentToAzureIdentity = addUserAppRoleAssignmentToAzureIdentity;
    }

    public async Task<AdministratorAppRoleAssignmentResponse> Handle(
        AdministratorAppRoleAssignmentCommand request, 
        CancellationToken cancellationToken)
    {
        if (configuredGraphServiceClient.Options.AdministratorUserId is not null)
        {
            await TryAssignUserRole(configuredGraphServiceClient.Options.AdministratorUserId, configuredGraphServiceClient.Options.AllUsersAppRoleId);
            await TryAssignUserRole(configuredGraphServiceClient.Options.AdministratorUserId, configuredGraphServiceClient.Options.AdministratorsAppRoleId);
        }

        return new();
    }

    private async Task TryAssignUserRole(string userID, string roleID)
    {
        try
        {
            await addUserAppRoleAssignmentToAzureIdentity.AddUserAppRoleAssignment(userID, roleID);
        }
        catch (ServiceException exception)
        {
            var expected = exception.InnerException is null
                && exception.StatusCode == System.Net.HttpStatusCode.BadRequest
                && exception.Error.Message == "Permission being assigned already exists on the object";

            // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.
            if (!expected)
            {
                throw new InvalidOperationException("Unexpected Service Error", exception);
            }
        }
        catch (Exception exception)
        {
            var expected = exception.InnerException is not null
                && exception.InnerException is ServiceException innerException
                && innerException.Error.Details is not null
                && innerException.Error.Details.First().Code == "InvalidUpdate";

            // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.
            if (!expected)
            {
                throw new InvalidOperationException("Unexpected Service Error", exception);
            }
        }
    }
}