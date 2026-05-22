using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class LogExportModel
    {
        [Display(Name = "#")]
        public int? ItemRecordID { get; set; }

        [Display(Name = "Item Category")]
        public string? ItemCategoryName { get; set; }

        [Display(Name = "Item Type")]
        public string? ItemTypeName { get; set; }

        [Display(Name = "Item Number")]
        public string? ItemNumber { get; set; }

        [Display(Name = "Start")]
        public DateTime? Start { get; set; }

        [Display(Name = "End")]
        public DateTime? End { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Billing Plan")]
        public string? PricingName { get; set; }

        [Display(Name = "Price")]
        public double? Price { get; set; }

        [Display(Name = "Billing Model")]
        public string? PricingType { get; set; }

        [Display(Name = "Record")]
        public int? RecordID { get; set; }
    }
}
