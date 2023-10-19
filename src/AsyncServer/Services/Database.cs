using Common.Models;

namespace AsyncServer.Services
{
    public class Database
    {
        private Dictionary<Guid, WeatherDatabaseItem> _weatherForecasts = new();

        public void UpsertForecast(Guid id, OperationStatus status, WeatherForecast forecast)
        {
            WeatherDatabaseItem item = GetOrCreate(id);
            item.Status = status;
            item.Data = forecast;

            _weatherForecasts[id] = item;
        }

        public WeatherDatabaseItem GetById(Guid id)
        {
            WeatherDatabaseItem result = null;
            _weatherForecasts.TryGetValue(id, out result);
            return result;
        }

        private WeatherDatabaseItem GetOrCreate(Guid id)
        {
            WeatherDatabaseItem result = null;

            if (!_weatherForecasts.TryGetValue(id, out result))
                return new WeatherDatabaseItem { RequestId = id };

            return result;
        }
    }
}
