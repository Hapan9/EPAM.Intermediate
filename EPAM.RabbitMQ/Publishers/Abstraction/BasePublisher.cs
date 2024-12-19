using EPAM.RabbitMQ.Publishers.Interfaces;
using RabbitMQ.Client;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Publishers.Abstraction
{
    public abstract class BasePublisher : IPublisher
    {
        protected readonly string Content;

        public BasePublisher(string content)
        {
            Content = content;
        }

        public abstract Task PublishMessageAsync(IConnection connection, CancellationToken cancellationToken = default);
    }
}
