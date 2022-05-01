using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public enum RentalType
    {

        [Display(Name = "Единоразовая")]
        OneTime,
        [Display(Name = "Почасовая")]
        Hourly,
        [Display(Name = "Посуточная")]
        Daily
    }
}