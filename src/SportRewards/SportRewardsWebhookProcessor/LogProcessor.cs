using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SportRewardsWebhookProcessor
{
    public class LogProcessor(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<LogProcessor>();

        [Function("LogProcessor")]
        public void Run([RabbitMQTrigger("webhooklog", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            _logger.LogInformation("Queue trigger {functionName} function processed: {myQueueItem}", nameof(LogProcessor), myQueueItem.Take(40).ToString());
        }
    }
}
