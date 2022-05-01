using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public abstract class RentalInfo
    {
        [Display(Name = "Статус")]
        public RentalStatus? RentalStatus { get; set; }

        [Display(Name = "Тарификация")]
        public RentalType RentalType { get; set; } = RentalType.Hourly;

        [Display(Name = "Начало")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? Start { get; set; }

        [Display(Name = "Завершение план")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? End { get; set; }

        [Display(Name = "Завершение факт")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? ActualEnd { get; set; }
        public static bool Overlap(RentalRecord a, RentalRecord b)
        {
            return (b.ActualEnd is not null || b.ActualEnd is not null) ? false :
                                            (a.Start < b.End &&
                                              a.End > b.Start);
        }
    }
}
