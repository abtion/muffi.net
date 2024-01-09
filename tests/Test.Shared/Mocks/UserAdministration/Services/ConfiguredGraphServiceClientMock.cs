using Domain.UserAdministration;
using Domain.UserAdministration.Services;
using Microsoft.Graph;
using System;

namespace Test.Shared.Mocks.UserAdministration.Services;

public class ConfiguredGraphServiceClientMock : IConfiguredGraphServiceClient
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "The property is not meant to be used in the mocks")]
    public GraphServiceClient Client => throw new NotImplementedException();

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "The property is not meant to be used in the mocks")]
    public AzureIdentityAdministrationOptions Options => throw new NotImplementedException();
}