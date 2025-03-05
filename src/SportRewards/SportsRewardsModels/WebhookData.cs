using MongoDB.Bson.Serialization.Attributes;
using SportsRewardsModels.EventData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SportsRewardsModels
{
    [BsonKnownTypes(typeof(CustomerData))]
    public class WebhookData<EventType> : IWebhookData
    {
        [Required]
        [MinLength(1)]
        public string? RequestId { get; set; }

        [Required]
        [MinLength(5)]
        public string? CustomerId { get; set; }

        [Required]
        [MinLength(1)]
        public string? Event { get; set; }

        [Required]
        public EventType? Data { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

    }
}
