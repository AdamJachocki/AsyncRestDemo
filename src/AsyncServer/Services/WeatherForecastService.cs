using Common.Models;

namespace AsyncServer.Services
{
    public class WeatherForecastService
    {
        private readonly Database _database;
        private readonly StorageQueueService _queueService;

        public WeatherForecastService(Database database, 
            StorageQueueService queueService)
        {
            _database = database;
            _queueService = queueService;
        }
        public async Task GetForecast(Guid requestId, DateOnly date)
        {
            _database.UpsertForecast(requestId, OperationStatus.InProgress, null);

            await Task.Delay(30000);

            var result = new WeatherForecast
            {
                Date = date,
                Summary = "Sunny",
                TemperatureC = 25
            };

            _database.UpsertForecast(requestId, OperationStatus.Finished, result);
        }

        public async Task SetForecastRequestToQueue(Guid requestId, DateOnly date)
        {
            WeatherForecast forecast = new WeatherForecast
            {
                Date = date
            };

            WeatherDatabaseItem dbItem = new WeatherDatabaseItem
            {
                RequestId = requestId,
                Data = forecast,
                Status = OperationStatus.NotStarted,
            };

            await _queueService.SendWeatherRequest(dbItem);
        }

        public async Task<WeatherForecast> GetReadyForecast(Guid requestId)
        {
            var item = _database.GetById(requestId);
            if (item == null)
                return null;

            return await Task.FromResult(item.Data);
        }

        public async Task<OperationStatus> GetRequestStatus(Guid requestId)
        {
            var status = await Task.Run(() =>
            {
                var item = _database.GetById(requestId);
                if (item == null)
                    return OperationStatus.NotStarted;
                else
                    return item.Status;
            });

            return status;
        }
    }
}
