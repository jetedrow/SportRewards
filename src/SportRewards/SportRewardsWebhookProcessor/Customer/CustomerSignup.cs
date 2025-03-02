using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SportRewardsWebhookProcessor.Customer
{
    public class CustomerSignup(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CustomerSignup>();

        [Function("CustomerSignup")]
        public void Run([RabbitMQTrigger("customer.signup", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            _logger.LogInformation("Queue trigger {functionName} function processed: {myQueueItem}", nameof(CustomerSignup), myQueueItem.Take(40).ToString());
        }
    }
}
