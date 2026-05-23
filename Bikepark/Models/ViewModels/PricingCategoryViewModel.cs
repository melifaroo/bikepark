using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models.ViewModels
{
    public class PricingCategoryViewModel
    {        
        public int PricingCategoryID { get; set; }

        [Required]
        [Display(Name = "Pricing Category")]
        public string? PricingCategoryName { get; set; }
    }
}