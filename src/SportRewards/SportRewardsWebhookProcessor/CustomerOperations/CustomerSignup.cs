using System;
using System.Text.Json;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using SportsRewardsModels;
using SportsRewardsModels.DataModels;
using SportsRewardsModels.EventData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportRewardsWebhookProcessor.CustomerOperations
{
    public class CustomerSignup(ILoggerFactory loggerFactory, IMongoDatabase database)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CustomerSignup>();
        private readonly IMongoDatabase database = database;

        [Function("CustomerSignup")]
        public async Task RunAsync([RabbitMQTrigger("customer.signup", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            WebhookData<CustomerData>? signupEvent = null;

            try
            {
                signupEvent = JsonSerializer.Deserialize<WebhookData<CustomerData>>(myQueueItem, CommonElements.DefaultJsonSerializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing customer request: {data}", myQueueItem);
            }

            if (signupEvent is not null)
            {
                var customerCollection = database.GetCollection<Customer>("customers");

                var filter = Builders<Customer>.Filter.Eq(c => c.CustomerId, signupEvent.CustomerId);

                var exists = await customerCollection.Find(filter).AnyAsync();

                if (exists)
                {
                    // Log an error - signup should be only inserting.
                    _logger.LogError("Duplicate sign-up detected for customerId {customerId}", signupEvent.CustomerId);
                }
                else
                {
                    // Map event to new customer.
                    var cust = new Customer()
                    {
                        CustomerId = signupEvent.CustomerId,
                        FirstName = signupEvent.Data?.FirstName,
                        LastName = signupEvent.Data?.LastName,
                        StreetAddress = signupEvent.Data?.StreetAddress,
                        AddressLine2 = signupEvent.Data?.AddressLine2,
                        City = signupEvent.Data?.City,
                        State = signupEvent.Data?.State,
                        ZipCode = signupEvent.Data?.ZipCode,
                        Email = signupEvent.Data?.Email
                    };

                    await customerCollection.InsertOneAsync(cust);


                    // Do additional signup processing events.


                }
            }
            _logger.LogInformation("Queue trigger {functionName} function processed: {myQueueItem}", nameof(CustomerSignup), myQueueItem.Take(40).ToString());


        }
    }
}
