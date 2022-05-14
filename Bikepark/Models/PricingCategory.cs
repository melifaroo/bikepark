using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class PricingCategory
    {
        public int PricingCategoryID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Категория тарификации")]
        public string? PricingCategoryName { get; set; }
    }
}
