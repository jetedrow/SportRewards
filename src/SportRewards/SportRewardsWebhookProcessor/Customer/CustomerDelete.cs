using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SportRewardsWebhookProcessor.Customer
{
    public class CustomerDelete(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CustomerDelete>();

        [Function("CustomerDelete")]
        public void Run([RabbitMQTrigger("customer.delete", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            _logger.LogInformation("Queue trigger {functionName} function processed: {myQueueItem}", nameof(CustomerDelete), myQueueItem.Take(40).ToString());
        }
    }
}
