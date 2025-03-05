using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsRewardsModels.DataModels
{
    public class EmailRequest
    {
        public string? CustomerId { get; set; }
        public string? TemplateName { get; set; }
        public Dictionary<string, string> TemplateData { get; set; } = [];
    }
}
