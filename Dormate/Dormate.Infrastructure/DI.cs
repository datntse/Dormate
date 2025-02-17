using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Repositories;
using Dormate.Infrastructure.Services;
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
            services.AddScoped<IEfTransaction, EfTransaction>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomService, RoomService>();

            services.AddScoped<IRoomRegisterRepository, RoomRegisterRepository>();
            services.AddScoped<IRoomRegisterService, RoomRegisterService>();

            services.AddScoped<IRoomImageRepository, RoomImageRepository>();
            services.AddScoped<IRoomImageService, RoomImageService>();

            services.AddScoped<IFavouriteRoomRepository, FavouriteRoomRepository>();
            services.AddScoped<IFavouriteRoomService, FavouriteRoomService>();

            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewService, ReviewService>();

            return services;
        }
        }
}
