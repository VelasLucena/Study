using Elastic.Apm.NetCoreAll;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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
using SudyApi.Startup;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

string connectStringSudyData = builder.Configuration.GetConnectionString(nameof(Database.SudyData));

builder.Services.AddDbContext<SudyContext>(options => options.UseMySql(connectStringSudyData
    , new MySqlServerVersion(new Version(8, 0, 31))
    , x => x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddScoped<ISudyService, SudyService>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddElasticSearch();

builder.Services.AddRedis();

builder.AddLoggerSystem();

builder.Services.AddJwt();

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddCors();

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    NullValueHandling = NullValueHandling.Ignore
};

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAllElasticApm(builder.Configuration);

app.UseMiddleware<AuthorizationTokenMiddleware>();

//app.UseMiddleware<InputMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}"
    );

app.MapControllers();

app.Run();
