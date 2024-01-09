namespace Api.Standalone;

public static class RegisterServices
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        // controller classes are not added to the IoC container by default
        services.AddControllers();



        return services;
    }
}