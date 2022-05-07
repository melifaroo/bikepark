using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemAge
    {
        [Display(Name = "")]
        Adult,
        [Display(Name = "Подросток")]
        Teen,
        [Display(Name = "Ребенок")]
        Kid,
    }
}

