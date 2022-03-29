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
            Categories = items.DistinctBy(item => item.CategoryID).Select(item => new SelectListItem { Value = item.CategoryID.ToString(), Text = item.Category?.Name ?? "" }).ToList();
            SubCategories = items.DistinctBy(item => item.SubCategoryID).Select(item => new SelectListItem { Value = item.SubCategoryID.ToString(), Text = item.SubCategory?.Name ?? "" }).ToList();
            Names = items.DistinctBy(item => item.Name).Select(item => new SelectListItem { Value = item.Name, Text = item.Name }).ToList();
            Numbers = items.DistinctBy(item => item.Number).Select(item => new SelectListItem { Value = item.Number.ToString(), Text = item.Number.ToString() }).ToList();
            Sizes = items.DistinctBy(item => item.Size).Select(item => new SelectListItem { Value = item.Size.ToString(), Text = item.Size.ToString() }).ToList();
            Statuses = items.DistinctBy(item => item.Status).Select(item => new SelectListItem { Value = item.Status.ToString(), Text = item.Status.ToString() }).ToList();

        }
    }
}
