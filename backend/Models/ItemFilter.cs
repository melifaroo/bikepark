using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bikepark.Models
{
    public class ItemFilter : Item
    {
        public List<SelectListItem> SubCategories { set; get; }
        public List<SelectListItem> Categories { set; get; }
        public List<SelectListItem> Names { set; get; }
        public List<SelectListItem> Numbers { set; get; }
        public List<SelectListItem> Sizes { set; get; }
        public List<SelectListItem> Statuses { set; get; }

        public ItemFilter(IEnumerable<Item> items)
        {
            Categories = items.DistinctBy(item => item.ItemCategoryID).Select(item => new SelectListItem { Value = item.ItemCategoryID.ToString(), Text = item.ItemCategory?.Name ?? "" }).ToList();
            SubCategories = items.DistinctBy(item => item.ItemSubCategoryID).Select(item => new SelectListItem { Value = item.ItemSubCategoryID.ToString(), Text = item.ItemSubCategory?.Name ?? "" }).ToList();
            Names = items.DistinctBy(item => item.ItemName).Select(item => new SelectListItem { Value = item.ItemName, Text = item.ItemName }).ToList();
            Numbers = items.DistinctBy(item => item.ItemNumber).Select(item => new SelectListItem { Value = item.ItemNumber, Text = item.ItemNumber }).ToList();
            Sizes = items.DistinctBy(item => item.ItemSize).Select(item => new SelectListItem { Value = item.ItemSize, Text = item.ItemSize }).ToList();
            Statuses = items.DistinctBy(item => item.ItemStatus).Select(item => new SelectListItem { Value = item.ItemStatus.ToString(), Text = item.ItemStatus.ToString() }).ToList();

        }
    }
}
