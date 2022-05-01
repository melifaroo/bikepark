using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemStatus
    {
        [Display(Name = "Доступен")]
        Available,
        [Display(Name = "Есть брони")]
        Reserved,
        [Display(Name = "В прокате")]
        RentedOut,
        [Display(Name = "На ремонте")]
        OnService
    }
}

