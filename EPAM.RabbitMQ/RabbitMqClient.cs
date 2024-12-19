using EPAM.RabbitMQ.Interfaces;
using EPAM.RabbitMQ.Publishers.Strategy.Interfaces;
using RabbitMQ.Client;
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

        public async Task<IConnection> GetConnection(CancellationToken cancellationToken = default)
        {
            if (_connection != null) return _connection;

            var connection = await _factory.CreateConnectionAsync(cancellationToken).ConfigureAwait(false);

            connection.ConfigureAwait(true);

            return connection;
        }

        public async Task PublishMessage(IStrategy strategy, CancellationToken cancellationToken = default)
        {
            var connection = await GetConnection().ConfigureAwait(false);
            var publisher = strategy.CreatePublisher();
            await publisher.PublishMessageAsync(connection, cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
