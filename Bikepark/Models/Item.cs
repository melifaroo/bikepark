using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class Item
    {
        public int? ItemID { get; set; }
        public int ItemTypeID { get; set; }

        [Display(Name = "������������")]
        public virtual ItemType? ItemType { get; set; }
        [Required]
        [Display(Name = "�����")]
        public string? ItemNumber { get; set; }

        [Display(Name = "������")]
        public ItemStatus? ItemStatus { get; set; } = Models.ItemStatus.Available;
        
    }

}