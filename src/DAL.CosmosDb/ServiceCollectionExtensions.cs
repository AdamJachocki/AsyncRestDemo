using Common.Options;
using DAL.CosmosDb.Repositories;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DAL.CosmosDb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, IConfiguration cosmosConfigSection) 
        {
            services.Configure<CosmosOptions>(cosmosConfigSection);

            services.AddSingleton(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CosmosOptions>>().Value;
                var builder = new CosmosClientBuilder(options.ConnectionString);
                builder = builder.WithCustomSerializer(new CosmosDbSerializer());

                return builder.Build();
            });

            services.AddSingleton<WeatherRepository>();
            return services;
        }
    }
}
