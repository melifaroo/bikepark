using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemAge
    {
        [Display(Name = "")]
        Adult,

        [Display(Name = "Teenage")]
        Teen,

        [Display(Name = "Children's")]
        Kid,
    }
}

