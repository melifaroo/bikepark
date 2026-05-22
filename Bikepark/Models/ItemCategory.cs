using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemCategory
    {
        public int ItemCategoryID { get; set; }

        [Required]
        [Display(Name = "Item Category")]
        public string? ItemCategoryName { get; set; }

        [Display(Name = "Accessories")]
        public bool Accessories { get; set; } = false;

    }
}
