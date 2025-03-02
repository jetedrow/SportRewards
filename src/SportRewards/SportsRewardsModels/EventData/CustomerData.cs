using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsRewardsModels.EventData
{
    public class CustomerData
    {
        [Required]
        [MinLength(1)]
        public string? FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string? LastName { get; set; }

        [Required]
        [MinLength(5)]
        public string? StreetAddress { get; set; }

        public string? AddressLine2 { get; set; }

        [Required]
        [MinLength(3)]
        public string? City { get; set; }

        [Required]
        [Length(2,2, ErrorMessage = $"{nameof(State)} must the USPS 2-character abbreviation.")]
        public string? State { get; set; }

        [Required]
        [Length(5,10)]
        public string? ZipCode { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
