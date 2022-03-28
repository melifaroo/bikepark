namespace Bikepark.Models
{
    public class ItemSubCategory
    {
        
        public int ItemSubCategoryID { get; set;}

        public string Name { get; set; }
        
        public int ItemCategoryID { get; set;}
        public ItemCategory ItemCategory { get; set;}
            
    }
}