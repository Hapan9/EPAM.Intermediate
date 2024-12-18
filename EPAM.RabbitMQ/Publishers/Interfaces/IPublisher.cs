using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Publishers.Interfaces
{
    public interface IPublisher
    {
        Task PublishMessageAsync(IConnection connection, CancellationToken cancellationToken = default);
    }
}
