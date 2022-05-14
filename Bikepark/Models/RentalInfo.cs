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

        [Display(Name = "Начало", ShortName = "C")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? Start { get; set; }

        [Display(Name = "Спланирован до", ShortName = "До")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? End { get; set; }


        [Display(Name = "Завершение", ShortName = "Закрыт")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime? EndActual { get; set; }

        public static bool Overlap(RentalInfo a, RentalInfo b)
        {
            return (a.RentalStatus == Models.RentalStatus.Closed || b.RentalStatus == Models.RentalStatus.Closed ) ? false : 
                                            (a.Start <= (b.RentalStatus == Models.RentalStatus.Active? MaxDT(b.End ?? DateTime.Now, DateTime.Now).AddHours(1) :b.End) &&
                                              (a.RentalStatus == Models.RentalStatus.Active ? MaxDT(a.End ?? DateTime.Now, DateTime.Now).AddHours(1) : a.End) >= b.Start);
        }

        public static DateTime MaxDT(DateTime date1, DateTime date2) {
            return (date1 > date2 ? date1 : date2);
        }

    }
}
