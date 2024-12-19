using EPAM.RabbitMQ.Publishers.Interfaces;

namespace EPAM.RabbitMQ.Publishers.Strategy.Interfaces
{
    public interface IStrategy
    {
        IPublisher CreatePublisher();
    }
}
