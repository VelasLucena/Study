using Nest;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Startup
{
    public static class ElasticSearchConfig
    {
        public static void AddElasticSearch(this IServiceCollection services)
        {
            string baseUrl = AppSettings.GetKey(ConfigKeys.ElasticSearchUrl);

            ConnectionSettings settings = new ConnectionSettings(new Uri(baseUrl ?? ""))
                .PrettyJson()
                .CertificateFingerprint("b159840547ddc0c9f9928aa71dbe7dd9d395c649842dbdac105480a6b4a82bdd")
                .BasicAuthentication("elastic", "5LxEc2Z_QR_mRuS4PC4c")
                .DefaultIndex("Teste");

            settings.EnableApiVersioningHeader();

            //AddDefaultMappings(settings);

            ElasticClient client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndexResponse createIndexResponse = client.Indices.Create("Teste", index => index.Map<InputModel>(x => x.AutoMap()));
        }

        //private static void AddDefaultMappings(ConnectionSettings settings)
        //{
        //    settings.DefaultMappingFor<InputModel>(m => m.Ignore(p => p.Link).Ignore(p => p.AuthorLink));
        //}
    }
}
