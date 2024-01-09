using System;

namespace Domain.UserAdministration.Exceptions;

public class AzureUserNotFoundException : Exception
{
    public AzureUserNotFoundException(string userId)
        : base($"User with id {userId} not found in Azure AD") { }
}
