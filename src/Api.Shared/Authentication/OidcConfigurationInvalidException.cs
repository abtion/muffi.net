namespace Api.Shared.Authentication.OpenIdConnect;

public class OidcConfigurationInvalidException : Exception {
    public OidcConfigurationInvalidException(string? message) : base(message) {
        // skip
    }
}