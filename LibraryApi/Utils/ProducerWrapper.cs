using Confluent.Kafka;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryApi.Utils
{
    public class ProducerWrapper
    {
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;
        private readonly ProducerConfig _config;

        public ProducerWrapper(string topic, ProducerConfig config)
        {
            _topic = topic;
            _config = config;
            _producer = new ProducerBuilder<Null, string>(_config).Build();
        }

        public async Task WriteMessage<T>(T message)
        {
            var options = new JsonSerializerOptions();
            options.IgnoreNullValues = true;
            options.Converters.Add(new JsonStringEnumConverter());
            var serializedData = JsonSerializer.Serialize(message, options);
            await _producer.ProduceAsync(_topic, new Message<Null, string>
            {
                Value = serializedData
            });
        }
    }
}
