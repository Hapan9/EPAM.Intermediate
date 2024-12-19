using EPAM.RabbitMQ.Publishers.Strategy.Interfaces;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Interfaces
{
    public interface IRabbitMqClient
    {
        Task PublishMessage(IStrategy strategy, CancellationToken cancellationToken = default);
        Task<IConnection> GetConnection(CancellationToken cancellationToken = default);
    }
}
