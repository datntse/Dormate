using Dormate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Dormate.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine);   // Log queries and errors to console
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransaction, EfTransaction>();

            return services;
        }
        }
}
