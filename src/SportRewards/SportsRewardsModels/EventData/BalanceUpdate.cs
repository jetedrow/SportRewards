using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsRewardsModels.EventData
{
    public class BalanceUpdate
    {
        [Required]
        public List<AccountBalance>? Balances { get; set; }
    }

    public class AccountBalance
    {
        [Required]
        public string? AccountType { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public string? Unit { get; set; }
    }
}
