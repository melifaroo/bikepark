namespace Bikepark.Models.ViewModels
{
    public class ItemRecordViewModel : RecordInfoViewModel
    {
        
        public int? ItemRecordID { get; set; }

        public int? RecordID { get; set; }

        public int? ItemID { get; set; }

        public string? ItemNumber { get; set; } = "";

        public string? ItemTypeName { get; set; } = "";

        public int? PricingCategoryID { get; set; }

        public int? PricingID { get; set; }
        
        public PricingViewModel? Pricing { get; set; }
        
    }
}