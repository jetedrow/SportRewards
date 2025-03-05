using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SportsRewardsModels;
using SportsRewardsModels.DataModels;

namespace SportRewardsWebhookProcessor.CommonWorkflow
{
    public class EmailCustomer(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<EmailCustomer>();

        [Function("EmailCustomer")]
        public void Run([RabbitMQTrigger("email", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            var request = JsonSerializer.Deserialize<EmailRequest>(myQueueItem, CommonElements.DefaultJsonSerializerOptions);

            if (request is not null)
            {
                // Make some sort of call to e-mail system such as SES, SendGrid, etc.

                _logger.LogInformation("Email {template} sent for customer {customerId}", request.TemplateName, request.CustomerId);
            }
            else
            {
                _logger.LogError("Email processing error: null request received.");
            }


        }
    }
}
