
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration config)
        {
            Services.AddScoped<IUserRepository, UserRepository>();
            Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            Services.AddScoped<ITokenService, TokenService>();
            Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return Services;
        }
    }
}