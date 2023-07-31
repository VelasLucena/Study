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
                .CertificateFingerprint("6b6a8c2ad2bc7b291a7363f7bb96a120b8de326914980c868c1c0bc6b3dc41fd")
                .BasicAuthentication("elastic", "JbNb_unwrJy3W0OaZ07n")
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
