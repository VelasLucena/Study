using Microsoft.EntityFrameworkCore;
using StudandoApi.Data.Contexts;
using StudandoApi.Data.Interfaces;
using SudyApi.Data.Services;
using SudyApi.Middlewares;
using SudyApi.Properties.Enuns;

var builder = WebApplication.CreateBuilder(args);

string connectStringSudyData = builder.Configuration.GetConnectionString(nameof(Database.SudyData));

builder.Services.AddDbContext<SudyContext>(options => options.UseMySql(connectStringSudyData, new MySqlServerVersion(new Version(8, 0, 31))));

builder.Services.AddScoped<ISudyService, SudyService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<AuthorizationTokenMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
