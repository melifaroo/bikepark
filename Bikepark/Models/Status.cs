using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum Status
    {
        [Display(Name = "Не назначен")]
        Draft,      
        [Display(Name = "Запланирован")]
        Scheduled,   
        [Display(Name = "Выдан")]
        Active,     
        [Display(Name = "Завершен")]
        Closed,

        [Display(Name = "Нужен ремонт")]
        Service,
        [Display(Name = "Ремонт")]
        OnService,
        [Display(Name = "Отремонтирован")]
        Fixed,     
    }
}

