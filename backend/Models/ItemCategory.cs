using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemCategory
    {

        public int ItemCategoryID { get; set;}

        public string Name { get; set;}

        [ForeignKey("ItemCategoryID")]
        public List<ItemSubCategory> ItemSubCategories { get; set; } 

    }
}
