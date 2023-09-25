﻿using Microsoft.Graph;
using System.Threading.Tasks;

namespace DomainModel.UserAdministration.Services;

public interface IUpdateUserInAzureIdentity
{
    public Task UpdateUser(User user);
}

public class UpdateUserInAzureIdentity : IUpdateUserInAzureIdentity
{
    private readonly IConfiguredGraphServiceClient client;

    public UpdateUserInAzureIdentity(IConfiguredGraphServiceClient configuredGraphServiceClient)
    {
        client = configuredGraphServiceClient;
    }

    public async Task UpdateUser(User user)
    {
        await client.Client.Users[user.Id].Request().UpdateAsync(user);
    }
}
