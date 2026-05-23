
using Bikepark.Models;
using Bikepark.Models.ViewModels;

namespace Bikepark.Mappers
{
    
    public static class ItemRecordMappingExtensions
    {
        
        public static ItemRecordViewModel ToViewModel( this ItemRecord itemRecord )
        { 
            return new ItemRecordViewModel
            {
                ItemRecordID  = itemRecord.ItemRecordID,
                RecordID = itemRecord.RecordID,
                ItemID = itemRecord.ItemID,
                ItemNumber = itemRecord.Item?.ItemNumber,
                ItemTypeName = itemRecord.Item?.ItemNumber,
                PricingCategoryID = itemRecord.Item?.ItemType?.PricingCategoryID,
                PricingID = itemRecord.PricingID,
                Pricing = itemRecord.Pricing?.ToViewModel()  
            };
        }
    }

}