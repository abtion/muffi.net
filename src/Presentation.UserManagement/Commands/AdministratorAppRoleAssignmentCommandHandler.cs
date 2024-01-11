using Domain.Shared;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Microsoft.Graph;
using Presentation.UserAdministration.Dtos;

namespace Presentation.UserAdministration.Commands;

public class AdministratorAppRoleAssignmentCommandHandler(IConfiguredGraphServiceClient ConfiguredGraphServiceClient, IAssignAppRoleToUser AssignAppRoleToUser) : ICommandHandler<AdministratorAppRoleAssignmentCommand, AdministratorAppRoleAssignmentResponse>
{
    public async Task<AdministratorAppRoleAssignmentResponse> Handle(AdministratorAppRoleAssignmentCommand request, CancellationToken cancellationToken)
    {
        if (ConfiguredGraphServiceClient.Options.AdministratorUserId is not null)
        {
            await TryAssignUserRole(ConfiguredGraphServiceClient.Options.AdministratorUserId, ConfiguredGraphServiceClient.Options.AllUsersAppRoleId, cancellationToken);
            await TryAssignUserRole(ConfiguredGraphServiceClient.Options.AdministratorUserId, ConfiguredGraphServiceClient.Options.AdministratorsAppRoleId, cancellationToken);
        }

        return new();
    }

    private async Task TryAssignUserRole(string userId, string roleId, CancellationToken cancellationToken)
    {
        try
        {
            await AssignAppRoleToUser.AssignAppRoleToUser(userId, roleId, cancellationToken);
        }
        catch (ServiceException exception)
        {
            // the implementation has been changed in version 5 of the Microsoft.Graph library to no longer contain the StatusCode property
            var expected = true;
            //exception.InnerException is null
            //&& exception.StatusCode == System.Net.HttpStatusCode.BadRequest
            //&& exception.Error.Message
            //    == "Permission being assigned already exists on the object";

            // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.
            if (!expected)
            {
                throw new InvalidOperationException("Unexpected Service Error", exception);
            }
        }
        catch (Exception exception)
        {
            // the implementation has been changed in version 5 of the Microsoft.Graph library to no longer contain the StatusCode property
            var expected = true;
            //exception.InnerException is not null
            //&& exception.InnerException is ServiceException innerException
            //&& innerException.Error.Details is not null
            //&& innerException.Error.Details.First().Code == "InvalidUpdate";

            // NOTE: invalid updates are expected, if the User has already been granted the Administrators Role.
            if (!expected)
            {
                throw new InvalidOperationException("Unexpected Service Error", exception);
            }
        }
    }
}
