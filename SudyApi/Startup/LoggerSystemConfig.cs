using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Serilog;
using Serilog.Exceptions;
using StudandoApi.Data.Contexts;

namespace SudyApi.Startup
{
    public static class LoggerSystemConfig
    {
        public static void AddLoggerSystem(this WebApplicationBuilder builder)
        {
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
        }
    }
}
