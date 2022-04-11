using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class Item 
    {
        public int ItemID { get; set; } = -1;

        [Required]
        [Display(Name = "Номер")]
        public string ItemNumber { get; set; } = string.Empty;

        public string? ItemName { get; set; } = string.Empty;

        public string? ItemDescription { get; set; } = string.Empty;

        public ItemStatus? ItemStatus { get; set; } = Models.ItemStatus.Ready;
        
        public int? ItemCategoryID {get; set;}

        public virtual ItemCategory? ItemCategory {get; set;}

        public int? ItemSubCategoryID {get; set;}

        public virtual ItemSubCategory? ItemSubCategory {get; set;}

        public string? ItemSize { get; set; }

        public string? ItemImageURL { get; set; }

    }

}