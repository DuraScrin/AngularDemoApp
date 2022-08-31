
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services, IConfiguration config)
        {
            //Authentication (Section 4 lesson 47). Nuget: Microsoft.AspNetCore.Authentication.JwtBearer
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false, //<-- API server
                        ValidateAudience = false //<-- Angular application
                    };
                }); 

            return Services;
        }
    }
}