using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsRewardsModels.EventData
{
    public class RewardsTransaction
    {
        [Required]
        public string? TransactionId { get; set; }

        public string? OrderId { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? TransactionType { get; set; }

        [Required]
        public TransactionAmount? Amount { get; set; }

        [Required]
        public AccountBalance? Balance { get; set; }

    }

    public class TransactionAmount
    {
        [Required]
        public string? AccountType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string? Unit { get; set; }
    }
}
