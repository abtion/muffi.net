﻿using Domain.UserAdministration.Entities;
using Domain.UserAdministration.Repositories;
using Domain.UserAdministration.Services;
using Microsoft.Graph.Models;

namespace Presentation.UserManagement.Repositories;

internal class AppRoleAssignmentRepository(IConfiguredGraphServiceClient ConfiguredGraphServiceClient) : IAssignAppRoleToUser, IRemoveAppRoleFromUser, IGetAppRoleAssignmentForUser, IGetAppRoleAssigment
{
    public async Task AssignAppRoleToUser(string userId, string appRoleId, CancellationToken cancellationToken)
    {
        var enterpriseApplicationObjectId = ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId;

        var assignment = new AppRoleAssignment
        {
            PrincipalId = Guid.Parse(userId),
            ResourceId = Guid.Parse(enterpriseApplicationObjectId),
            AppRoleId = Guid.Parse(appRoleId),
        };

        await ConfiguredGraphServiceClient.Client.ServicePrincipals[enterpriseApplicationObjectId].AppRoleAssignedTo.PostAsync(assignment, cancellationToken: cancellationToken);
    }

    public async Task<List<AppRoleAssignmentEntity>> GetAppRoleAssignments(CancellationToken cancellationToken)
    {
        var azureRoleAssignments = await ConfiguredGraphServiceClient.Client.ServicePrincipals[ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId].AppRoleAssignedTo.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Select =
            [
                "principalId",
                "principalDisplayName",
                "appRoleId"
            ];
        }, cancellationToken);

        if (azureRoleAssignments is null || azureRoleAssignments.Value is null || azureRoleAssignments.Value.Count == 0)
            return [];

        var result = azureRoleAssignments.Value;

        return result.Select(p => new AppRoleAssignmentEntity(p.Id, p.AppRoleId, p.PrincipalId, p.PrincipalDisplayName)).ToList();
    }

    public async Task<List<AppRoleAssignmentEntity>> GetAppRoleAssignmentsForUser(string userId, CancellationToken cancellationToken)
    {
        var roleAssignments = await ConfiguredGraphServiceClient.Client.Users[userId].AppRoleAssignments.GetAsync(requestConfiguration =>
        {
            requestConfiguration.QueryParameters.Top = 999;
            requestConfiguration.QueryParameters.Filter = $"resourceId eq {ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId}";
        }, cancellationToken);

        if (roleAssignments is null || roleAssignments.Value is null || roleAssignments.Value.Count == 0)
            return [];

        var result = roleAssignments.Value;

        return result.Select(p => new AppRoleAssignmentEntity(p.Id, p.AppRoleId, p.PrincipalId, p.PrincipalDisplayName)).ToList();
    }

    public async Task RemoveAppRoleFromUser(string appRoleAssignmentId, CancellationToken cancellationToken)
    {
        var graphClient = ConfiguredGraphServiceClient.Client;
        var enterpriseApplicationObjectId = ConfiguredGraphServiceClient.Options.EnterpriseApplicationObjectId;

        await graphClient.ServicePrincipals[enterpriseApplicationObjectId].AppRoleAssignedTo[appRoleAssignmentId].DeleteAsync(cancellationToken: cancellationToken);
    }
}
