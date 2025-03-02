using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SportsRewardsModels;
using SportsRewardsModels.EventData;
using System.Text.Json;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using RabbitMQ.Client;
using System.Text;

namespace SportRewardsWebhookReceiver
{
    public class Receiver(ILogger<Receiver> logger, IConnectionFactory factory)
    {
        private readonly ILogger<Receiver> _logger = logger;
        private readonly IConnectionFactory factory = factory;
        private readonly JsonSerializerOptions serOptions = new() {
            AllowOutOfOrderMetadataProperties = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Function("ReceiveWebhook")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", "put")] HttpRequest req)
        {
            IWebhookData? data;

            try
            {
                // Use polymorphic deserialization to get correct implementation based on $type of webhook.
                using var sr = new StreamReader(req.Body);
                string bodyData = await sr.ReadToEndAsync();
                data = JsonSerializer.Deserialize<IWebhookData>(bodyData, serOptions);
                List<ValidationResult> results = [];

                if (data is null)
                {
                    return new BadRequestResult();
                }

                // Use validator to validate data annotations to confirm model is correct.
                var isValid = Validator.TryValidateObject(data, new ValidationContext(data), results);

                if (!isValid)
                {
                    return new BadRequestObjectResult(new { Messages = results.Select(m => m.ErrorMessage) });
                }

                // Enqueue message for processing.
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync("webhooklog", true, false, false);
                await channel.QueueDeclareAsync(data?.Event ?? "unknown-event", false, false, false);

                // Publish message so it can be logged.
                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "webhooklog", mandatory: true, 
                    basicProperties: new BasicProperties {Persistent= true}, body: Encoding.UTF8.GetBytes(bodyData));

                // Publish message so processor can process asyncronously.
                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: data?.Event ?? "unknown-event", mandatory: true,
                    basicProperties: new BasicProperties { Persistent = true }, body: Encoding.UTF8.GetBytes(bodyData));

            }
            catch (JsonException jex)
            {
                return new BadRequestObjectResult(new { jex.Message });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { ex.Message, Type = ex.GetType().Name });
            }
            
            return new OkObjectResult(new { data?.RequestId, Result = "success" });
        }
    }
}
