using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.CosmosDb
{
    internal class CosmosDbSerializer : CosmosSerializer
    {
        private readonly JsonSerializerOptions _serializerOptions;

        public CosmosDbSerializer()
        {
            _serializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public override T FromStream<T>(Stream stream)
        {
            using (stream)
            {
                return JsonSerializer.Deserialize<T>(stream, _serializerOptions);
            }
        }

        public override Stream ToStream<T>(T input)
        {
            var memoryStream = new MemoryStream();

            JsonSerializer.Serialize(memoryStream, input, typeof(T), _serializerOptions);
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
