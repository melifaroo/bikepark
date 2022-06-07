using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemPrepared
    {
        public int ItemPreparedID { get; set; }
        public int ItemID { get; set; }

        [Display(Name = "Номер")]
        public virtual Item? Item { get; set; }

    }
}
