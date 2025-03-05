using SportsRewardsModels.EventData;
using System;
using System.Text.Json.Serialization;

namespace SportsRewardsModels
{
    [JsonDerivedType(typeof(WebhookData<CustomerData>),"customer")]
    [JsonDerivedType(typeof(WebhookData<object>))]
    public interface IWebhookData
    {
        string? CustomerId { get; set; }
        string? RequestId { get; set; }
        string? Event { get; set; }
        DateTimeOffset Timestamp { get; set; }
    }
}