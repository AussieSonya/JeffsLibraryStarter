using Confluent.Kafka;
using LibraryApi.Domain;
using LibraryApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class KafkaReservationProducer : ILogReservations
    {
        private readonly ProducerConfig _config;
        private readonly ProducerWrapper _producer;
        public KafkaReservationProducer(ProducerConfig config)
        {
            _config = config;
            _producer = new ProducerWrapper("libraryreservations", _config);
        }

        public async Task WriteAsync(Reservation reservation)
        {
            // What if the kafka server is down?
            await _producer.WriteMessage(reservation);
        }
    }
}
