using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using SportsRewardsModels;

namespace SportRewardsWebhookProcessor
{
    public class LogProcessor(ILoggerFactory loggerFactory, IMongoDatabase database)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<LogProcessor>();
        private readonly IMongoDatabase database = database;

        [Function("LogProcessor")]
        public async Task RunAsync([RabbitMQTrigger("webhooklog", ConnectionStringSetting = "MQConnectionString")] string myQueueItem)
        {
            var request = BsonDocument.Parse(myQueueItem);

            if (request is not null)
            {

                // Exchange $type provided in JSON for 'type' to avoid mongo naming issues.
                // Translate back if you need to serialize back to an IWebHookData.
                if (request.TryGetElement("$type", out BsonElement element)) 
                {
                    var newElem = new BsonElement("type", element.Value);
                    request.Remove("$type");
                    request.Add(newElem);

                }

                var logCollection = database.GetCollection<BsonDocument>("requestlogs");
                try
                {
                    await logCollection.InsertOneAsync(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error logging request {RequestId}", request.GetElement("requestId").Value ?? "UNKNOWN");
                }
            }

            _logger.LogInformation("Queue trigger {functionName} function processed: {myQueueItem}", nameof(LogProcessor), myQueueItem.Take(40).ToString());
        }
    }
}
