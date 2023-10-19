using Common.Models;
using Common.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace DAL.CosmosDb.Repositories
{
    public class WeatherRepository
    {
        private readonly Container _cosmosContainer;

        public WeatherRepository(CosmosClient cosmosClient, 
            IOptions<CosmosOptions> cosmosOptions)
        {
            var options = cosmosOptions.Value;
            _cosmosContainer = cosmosClient.GetContainer(options.DatabaseName, "operations");
        }

        public async Task UpsertWeatherOperation(WeatherDatabaseItem data)
        {
            await _cosmosContainer.UpsertItemAsync(data);
        }

        public async Task<WeatherDatabaseItem> GetWeatherData(Guid requestId)
        {
            var data = await _cosmosContainer.ReadItemAsync<WeatherDatabaseItem>(requestId.ToString(), new PartitionKey("/requestId"));
            return data.Resource;
        }
    }
}
