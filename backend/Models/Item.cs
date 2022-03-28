using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class Item 
    {
        public int ItemID { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Name { get; set; }

        public ItemStatus Status {get; set;}
        
        public int CategoryID {get; set;}
        public virtual ItemCategory Category {get; set;}

        public int SubCategoryID {get; set;}
        public virtual ItemSubCategory SubCategory {get; set;}
        
        public int Size { get; set; } 
    }

}