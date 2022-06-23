using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Item
    {
        public int? ItemID { get; set; }
        [Display(Name = "Модель")]
        public int? ItemTypeID { get; set; }

        [Display(Name = "Модель")]
        public virtual ItemType? ItemType { get; set; }
        [Required]
        [Display(Name = "Номер")]
        public string? ItemNumber { get; set; }

        public bool Archival { get; set; } = false;

    }

}