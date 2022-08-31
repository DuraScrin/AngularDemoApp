
namespace API.Extensions
{
    public static class ApplicationSecurityServices
    {
        public static IServiceCollection AddApplicationSecurityServices(this IServiceCollection Services, WebApplicationBuilder builder)
        {
            //Cross-Origin Resource Sharing (CORS)
            Services.AddCors(p => p.AddPolicy("corsapp", builder =>
            {
                builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyMethod().AllowAnyHeader();
            }));

            return Services;
        }
    }
}