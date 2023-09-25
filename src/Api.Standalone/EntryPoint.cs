using DomainModel.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Standalone
{
    public static class EntryPoint
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            // controller classes are not added to the IoC container by default
            services.AddControllers();

            return services;
        }

        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
                    )
            );

            return services;
        }
    }
}
