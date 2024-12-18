using EPAM.RabbitMQ.Publishers.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Interfaces
{
    public interface IRabbitMqClient
    {
        Task PublishMessage(params IPublisher[] publishers);
        Task PublishMessage(CancellationToken cancellationToken = default, params IPublisher[] publishers);
    }
}
