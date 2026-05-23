using Bikepark.Models;
using Bikepark.Models.ViewModels;

namespace Bikepark.Mappers
{
    
    public static class RecordMappingExtensions
    {
        
        public static RecordViewModel ToViewModel( this Record record )
        { 
            return new RecordViewModel
            {
                RecordID = record.RecordID,
                CustomerID = record.CustomerID,
                Customer = record.Customer?.ToViewModel(),
                ItemRecords = record.ItemRecords.Select( itemRecord => itemRecord.ToViewModel() ).ToList()                
            };
        }

    }

}