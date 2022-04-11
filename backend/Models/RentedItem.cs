namespace Bikepark.Models
{
    public class RentedItem
    {
        public int RentedItemID { get; set; }

        public int RentalRecordID { get; set; }

        public virtual RentalRecord? RentalRecord { get; set; }

        public int? ItemID { get; set; }

        public virtual Item? Item { get; set; }

        public int? RentalFareID { get; set; }

        public virtual RentalFare? RentalFare { get; set; }

        public RentalStatus RentalStatus { get; set; } = RentalStatus.New;

    }
}
