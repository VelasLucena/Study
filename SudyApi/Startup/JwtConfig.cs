using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SudyApi.Utility;
using System.Text;

namespace SudyApi.Startup
{
    public static class JwtConfig
    {
        public static void AddLoggerSystem(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(AppSettings.AppSetting["Settings:JwtKey"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });
        }
    }
}
