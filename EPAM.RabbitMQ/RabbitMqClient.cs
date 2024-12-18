using EPAM.RabbitMQ.Interfaces;
using EPAM.RabbitMQ.Publishers.Interfaces;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ
{
    public class RabbitMqClient : IRabbitMqClient
    {
        ConnectionFactory _factory;
        IConnection? _connection;

        public RabbitMqClient()
        {
            _factory = new ConnectionFactory
            {
                VirtualHost = "/",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
        }

        private async Task<IConnection> GetConnection(CancellationToken cancellationToken = default)
        {
            if (_connection != null) return _connection;

            var connection = await _factory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

            connection.ConfigureAwait(true);

            return connection;
        }

        public async Task PublishMessage(params IPublisher[] publishers)
        {
            var connection = await GetConnection().ConfigureAwait(false);
            List<Task> tasks = new List<Task>();
            foreach (var publisher in publishers)
            {
                var task = publisher.PublishMessageAsync(connection);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public async Task PublishMessage(CancellationToken cancellationToken = default, params IPublisher[] publishers)
        {
            var connection = await GetConnection(cancellationToken).ConfigureAwait(false);
            List<Task> tasks = new List<Task>();
            foreach (var publisher in publishers)
            {
                var task = publisher.PublishMessageAsync(connection, cancellationToken);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
