using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VHM.DAL.Data;
using VHM.DAL.Entities;

namespace VHM_APi_.Extentions
{
    public static class IdentityServices
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                
            })
            .AddEntityFrameworkStores<HospitalContext>();
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                    };
                });
        }
    }
}
