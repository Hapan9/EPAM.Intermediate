using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Consumers.Interfaces
{
    public interface IConsumer
    {
        Task<T> Consume<T>(CancellationToken cancellationToken = default);
    }
}
