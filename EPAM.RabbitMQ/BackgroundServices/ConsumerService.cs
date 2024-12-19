using EPAM.RabbitMQ.Consumers;
using EPAM.RabbitMQ.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.BackgroundServices
{
    public class ConsumerService : BackgroundService
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _services;

        public ConsumerService(IRabbitMqClient rabbitMqClient, ILoggerFactory loggerFactory, IServiceProvider services)
        {
            _rabbitMqClient = rabbitMqClient;
            _loggerFactory = loggerFactory;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new SmsConsumer(_loggerFactory);
            var connection = await _rabbitMqClient.GetConnection();
            await consumer.Consume(connection);
            //while (true) { }
        }
    }
}
