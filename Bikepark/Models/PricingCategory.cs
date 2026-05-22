using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class PricingCategory
    {
        public int PricingCategoryID { get; set; }

        [Required]
        [Display(Name = "Pricing Category")]
        public string? PricingCategoryName { get; set; }

    }
}
