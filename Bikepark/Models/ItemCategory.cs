using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemCategory
    {
        public int ItemCategoryID { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Категория")]
        public string? ItemCategoryName { get; set; }
        public bool Accessories { get; set; } = false;

    }
}
