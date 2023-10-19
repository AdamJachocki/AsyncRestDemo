using Azure.Storage.Queues;
using Common.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AsyncServer.Services
{
    public class StorageQueueService
    {
        private readonly QueueOptions _queueOptions;

        public StorageQueueService(IOptions<QueueOptions> queueOptions)
        {
            _queueOptions = queueOptions.Value;
        }
        public async Task SendWeatherRequest(WeatherDatabaseItem item)
        {
            QueueClientOptions clientOptions = new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            };

            var client = new QueueClient(_queueOptions.ConnectionString, 
                _queueOptions.WeatherQueueName, clientOptions);
            
            WeatherQueueItem queueItem = new WeatherQueueItem
            {
                Data = item
            };

            var serializedData = JsonSerializer.Serialize(queueItem);
            await client.SendMessageAsync(serializedData);
        }
    }
}
