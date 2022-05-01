using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemGender
    {
        [Display(ShortName = "-", Name = "Унисекс") ]
        Unisex,
        [Display(ShortName = "М", Name = "Мужской")]
        M,
        [Display(ShortName = "Ж", Name = "Женский")]
        W,
    }
}

