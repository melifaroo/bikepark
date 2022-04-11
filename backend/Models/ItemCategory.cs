using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public class ItemCategory
    {

        public int ItemCategoryID { get; set; } = -1;

        public string Name { get; set;} = string.Empty;

        [ForeignKey("ItemCategoryID")]
        public List<ItemSubCategory> ItemSubCategories { get; set; } = new List<ItemSubCategory>();

    }
}
