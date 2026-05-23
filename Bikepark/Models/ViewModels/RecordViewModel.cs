using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models.ViewModels
{
    public class RecordViewModel : RecordInfoViewModel
    {
        
        public int? RecordID { get; set; }

        public int? CustomerID { get; set; }

        [Display(Name = "Customer")]
        public CustomerViewModel? Customer { get; set; }
                
        public List<ItemRecordViewModel> ItemRecords { get; set; } = new();

    }
}