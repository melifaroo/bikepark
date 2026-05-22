using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemType 
    {
        public int? ItemTypeID { get; set; }

        [Display(Name = "Item Type")]
        public string? ItemTypeName { get; set; }

        [Display(Name = "Item Category")]
        public int? ItemCategoryID { get; set; }

        [Display(Name = "Item Category")]
        public virtual ItemCategory? ItemCategory { get; set; }

        [Display(Name = "Pricing Category")]
        public int? PricingCategoryID { get; set; }

        [Display(Name = "Pricing Category")]
        public virtual PricingCategory? PricingCategory { get; set; }

        [Display(Name = "Age")]
        public ItemAge? ItemAge { get; set; }

        [Display(Name = "Gender")]
        public ItemGender? ItemGender { get; set; }

        [Display(Name = "Size")]
        public ItemSize? ItemSize { get; set; }

        [Display(Name = "Wheel Size")]
        public string? ItemWheelSize { get; set; }

        [Display(Name = "Color")]
        public string? ItemColor { get; set; }

        [Display(Name = "Description")]
        public string? ItemDescription { get; set; }

        [Display(Name = "External Link")]
        public string? ItemExternalURL { get; set; }

        [Display(Name = "Image")]
        public string? ItemImageURL { get; set; }

        public bool Archival { get; set; } = false;

    }

}