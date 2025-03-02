using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SportRewardsWebhookProcessor.Customer
{
    public class CustomerUpdate(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CustomerUpdate>();

        [Function("CustomerUpdate")]
        public void Run([RabbitMQTrigger("customer.update", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            _logger.LogInformation("Queue trigger {functionName} function processed: {myQueueItem}", nameof(CustomerUpdate), myQueueItem.Take(40).ToString());
        }
    }
}
