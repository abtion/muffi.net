using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api.Shared.Authentication.OpenIdConnect;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOidcAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("Authentication:Authority");
                options.ClaimsIssuer = configuration.GetValue<string>("Authentication:ClaimsIssuer");
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = configuration.GetValue<bool>("Authentication:TokenValidationParameters:ValidateIssuer"),
                    ValidateAudience = configuration.GetValue<bool>("Authentication:TokenValidationParameters:ValidateAudience"),
                    ValidateLifetime = configuration.GetValue<bool>("Authentication:TokenValidationParameters:ValidateLifetime"),
                    ValidateTokenReplay = configuration.GetValue<bool>("Authentication:TokenValidationParameters:ValidateTokenReplay"),
                    ValidIssuer = configuration.GetValue<string>("Authentication:TokenValidationParameters:ValidIssuer"),
                    ValidAudience = configuration.GetValue<string>("Authentication:TokenValidationParameters:ValidAudience"),
                };
            });

        return services;
    }
}