using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class Item 
    {
        public int ItemID { get; set; }

        public string ItemNumber { get; set; }

        public string ItemName { get; set; }

        public ItemStatus? ItemStatus {get; set;}
        
        public int? ItemCategoryID {get; set;}

        public virtual ItemCategory? ItemCategory {get; set;}

        public int? ItemSubCategoryID {get; set;}
        public virtual ItemSubCategory? ItemSubCategory {get; set;}

        public string? ItemSize { get; set; }
        public string? ItemDescription { get; set; }
        public string? ItemImageURL { get; set; }

    }

}