using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemAge
    {
        [Display(Name = "Взрослый")]
        Adult,
        [Display(Name = "Подростковый")]
        Teen,
        [Display(Name = "Детский")]
        Kid,
    }
}

