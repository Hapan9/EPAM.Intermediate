using EPAM.EF.Entities;
using EPAM.RabbitMQ.Publishers.Interfaces;
using EPAM.RabbitMQ.Publishers.Strategy.Interfaces;

namespace EPAM.RabbitMQ.Publishers.Strategy.Abstraction
{
    public abstract class BaseStrategy : IStrategy
    {
        protected readonly Notification Notification;

        public BaseStrategy(Notification notification)
        {
            Notification = notification;
        }

        public abstract IPublisher CreatePublisher();
    }
}
