namespace Bikepark.Models
{
    public class ItemSubCategory
    {
        
        public int ItemSubCategoryID { get; set;}

        public string Name { get; set; } = string.Empty;

        public int? ItemCategoryID { get; set; } = -1;

        public ItemCategory? ItemCategory { get; set;} = null;
            
    }
}