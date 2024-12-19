using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EPAM.RabbitMQ.Consumers
{
    public class FakeConsumerService
    {
        private readonly ILogger<FakeConsumerService> _logger;

        public FakeConsumerService(ILogger<FakeConsumerService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmail(string content)
        {
            _logger.LogInformation($"Email sent\r\n{content}");
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public async Task SendSms(string content)
        {
            _logger.LogInformation($"Sms sent\r\n{content}");
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
