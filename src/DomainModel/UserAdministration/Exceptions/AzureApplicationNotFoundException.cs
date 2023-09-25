﻿using System;

namespace DomainModel.UserAdministration.Exceptions;

public class AzureApplicationNotFoundException : Exception
{
    public AzureApplicationNotFoundException()
        : base($"Application not found in Azure AD") { }
}
