using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using StudandoApi.Data.Interfaces;
using SudyApi.Data.Interfaces;
using SudyApi.Data.Services;
using SudyApi.Middlewares;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string connectStringSudyData = builder.Configuration.GetConnectionString(nameof(Database.SudyData));

builder.Services.AddDbContext<SudyContext>(options => options.UseMySql(connectStringSudyData, new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddScoped<ISudyService, SudyService>();
builder.Services.AddScoped<ICacheService, CacheService>();

#region RedisCache

builder.Services.AddStackExchangeRedisCache(x =>
{
    x.InstanceName = nameof(SudyContext);
    x.Configuration = $"localhost:6379";
});

#endregion

#region JwtBearer

var key = Encoding.ASCII.GetBytes(AppSettings.AppSetting["Settings:JwtKey"]);

builder.Services.AddAuthentication(x =>
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

#endregion

builder.Services.AddControllers();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

var app = builder.Build();

app.UseMiddleware<AuthorizationTokenMiddleware>();

app.UseMiddleware<InputMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}"
    );

app.MapControllers();

app.Run();
