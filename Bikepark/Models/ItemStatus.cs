using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemStatus
    {
        [Display(Name = "��������")]
        Available,
        [Display(Name = "���� �����")]
        Reserved,
        [Display(Name = "� �������")]
        RentedOut,
        [Display(Name = "�� �������")]
        OnService
    }
}

