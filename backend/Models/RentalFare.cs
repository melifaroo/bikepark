using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class RentalFare
    {

        public int RentalFareID { get; set; }

        public string? FareName  { get; set; }

        [ForeignKey("ItemID")]
        public List<Item> ApplicableFor { get; set; } = new List<Item>();

        public int MinTermInHours { get; set; } = 1;

        public RateDaysOfWeek DaysOfWeek { get; set; }

        public Boolean IsHoliday { get; set; }

        public Boolean IsReducedFare { get; set; }

        public double Fare { get; set; } = 1;


    }
}
