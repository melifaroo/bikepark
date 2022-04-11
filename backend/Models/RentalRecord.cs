using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class RentalRecord
    {

        public int RentalRecordID { get; set; }

        public RentalStatus RentalStatus { get; set; } = RentalStatus.New;

        public DateTime RentalBookingTime { get; set; }

        public DateTime RentalStartTime { get; set; }

        public DateTime RentalEndTime { get; set; }

        public int? CustomerID  { get; set; }

        public virtual Customer? Customer { get; set; }

        [ForeignKey("RentedItemID")]
        public List<RentedItem> RentedItems { get; set; } = new List<RentedItem>();

        public int RentingTermInHours { get; set; } = 1;

    }
}
