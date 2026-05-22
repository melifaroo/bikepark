using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class StorageExportModel
    {
        [Display(Name = "#")]
        public int? ItemID { get; set; }

        [Display(Name = "Number")]
        public string? ItemNumber { get; set; }

        [Display(Name = "Item Category")]
        public string? ItemCategoryName { get; set; }

        [Display(Name = "Item Type")]
        public string? ItemTypeName { get; set; }

        [Display(Name = "Pricing Category")]
        public string? PricingCategoryName { get; set; }

        [Display(Name = "Age")]
        public string? ItemAge { get; set; }

        [Display(Name = "Gender")]
        public string? ItemGender { get; set; }

        [Display(Name = "Size")]
        public string? ItemSize { get; set; }

        [Display(Name = "Wheel Size")]
        public string? ItemWheelSize { get; set; }

        [Display(Name = "Color")]
        public string? ItemColor { get; set; }

        [Display(Name = "Description")]
        public string? ItemDescription { get; set; }

        [Display(Name = "External Link")]
        public string? ItemExternalURL { get; set; }
    }
}
