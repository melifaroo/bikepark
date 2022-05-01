using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemGender
    {
        [Display(ShortName = "-", Name = "�������") ]
        Unisex,
        [Display(ShortName = "�", Name = "�������")]
        M,
        [Display(ShortName = "�", Name = "�������")]
        W,
    }
}

