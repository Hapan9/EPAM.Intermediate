using EPAM.RabbitMQ.Publishers.Interfaces;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Publishers.Abstraction
{
    public abstract class BasePublisher : IPublisher
    {
        protected readonly object PublishObject;

        public BasePublisher(object publishObject)
        {
            PublishObject = publishObject;
        }

        public abstract Task PublishMessageAsync(IConnection connection, CancellationToken cancellationToken = default);
    }
}
