using Azure.Storage.Queues.Models;
using Common.Models;
using DAL.CosmosDb.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public class AnalyzeWeather
    {
        private readonly ILogger<AnalyzeWeather> _logger;
        private readonly WeatherRepository _weatherRepository;
        private readonly Randomizer _randomizer;

        public AnalyzeWeather(ILogger<AnalyzeWeather> logger, 
            WeatherRepository weatherRepository, 
            Randomizer randomizer)
        {
            _logger = logger;
            _weatherRepository = weatherRepository;
            _randomizer = randomizer;
        }

        [Function(nameof(AnalyzeWeather))]
        public async Task Run([QueueTrigger("weather-requests-queue", Connection = "QueueConnectionString")] QueueMessage message)
        {
            _logger.LogInformation("Weather analyzing started");

            WeatherQueueItem msgItem = message.Body.ToObjectFromJson<WeatherQueueItem>();

            WeatherDatabaseItem dbItem = new WeatherDatabaseItem();
            dbItem.Status = OperationStatus.InProgress;
            dbItem.RequestId = msgItem.Data.RequestId;
            dbItem.Data = msgItem.Data.Data;

            await _weatherRepository.UpsertWeatherOperation(dbItem);

            //symulacja
            await Task.Delay(30000);

            dbItem.Data.Summary = "Warm";
            dbItem.Data.TemperatureC = _randomizer.GetInt(20, 29);
            dbItem.Status = OperationStatus.Finished;

            await _weatherRepository.UpsertWeatherOperation(dbItem);            
        }
    }
}
