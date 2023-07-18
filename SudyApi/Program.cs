using Elastic.Apm.NetCoreAll;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Serilog.Exceptions;
using StudandoApi.Data.Contexts;
using StudandoApi.Data.Interfaces;
using SudyApi.Data.Interfaces;
using SudyApi.Data.Services;
using SudyApi.Middlewares;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

string connectStringSudyData = builder.Configuration.GetConnectionString(nameof(Database.SudyData));

builder.Services.AddDbContext<SudyContext>(options => options.UseMySql(connectStringSudyData, new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddScoped<ISudyService, SudyService>();
builder.Services.AddScoped<ICacheService, CacheService>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .Enrich.WithEnvironmentUserName()
    .Enrich.WithExceptionDetails()
    .Enrich.WithElasticApmCorrelationInfo()
    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticApmSettings:Url"]))
    {
        CustomFormatter = new EcsTextFormatter(),
        AutoRegisterTemplate = true,
        ModifyConnectionSettings = x => x.BasicAuthentication(builder.Configuration["ElasticApmSettings:UserName"], builder.Configuration["ElasticApmSettings:Password"])
    })
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

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

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
};

var app = builder.Build();

app.UseAllElasticApm(builder.Configuration);

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
