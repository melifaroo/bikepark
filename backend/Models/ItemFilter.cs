using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bikepark.Models
{
    public class ItemFilter
    {
        public string? ItemNumber { get; set; } = null;
        public ItemStatus? ItemStatus { get; set; } = null;
        public string? ItemName { get; set; } = null;
        public int? ItemCategoryID { get; set; }
        public int? ItemSubCategoryID { get; set; }
        public ItemSize? ItemSizing { get; set; }
        public List<SelectListItem>? ItemCategories { set; get; }
        public List<SelectListItem>? ItemNames { set; get; }
        public List<SelectListItem>? ItemNumbers { set; get; }
        public List<SelectListItem>? ItemSizes { set; get; }
        public List<SelectListItem>? ItemStatuses { set; get; }

        public ItemFilter() { }

        public ItemFilter(IEnumerable<Item> items)
        {
            ItemCategories = items.DistinctBy(item => item.ItemTypeID).DistinctBy(item => item.ItemType.ItemCategoryID).Select(item => new SelectListItem { Value = item.ItemType.ItemCategoryID.ToString(), Text = item.ItemType.ItemCategory?.ItemCategoryName ?? "" }).ToList();
            ItemNames = items.DistinctBy(item => item.ItemTypeID).DistinctBy(item => item.ItemType.ItemTypeName).Select(item => new SelectListItem { Value = item.ItemType.ItemTypeName, Text = item.ItemType.ItemTypeName }).ToList();
            ItemSizes = items.DistinctBy(item => item.ItemTypeID).DistinctBy(item => item.ItemType.ItemSize).Where(item => item.ItemType.ItemSize != null).Select(item => new SelectListItem { Value = item.ItemType.ItemSize.ToString(), Text = item.ItemType.ItemSize.ToString() } ).ToList();
            ItemNumbers = items.DistinctBy(item => item.ItemNumber).Select(item => new SelectListItem { Value = item.ItemNumber, Text = item.ItemNumber }).ToList();
            ItemStatuses = items.DistinctBy(item => item.ItemStatus).Select(item => new SelectListItem { Value = item.ItemStatus.ToString(), Text = item.ItemStatus.ToString() }).ToList();
        }

        public List<Item> FilterList(IEnumerable<Item> items)
        {
            var filtered = items;
            if (ItemCategoryID != null)
                filtered = filtered.Where(x => x.ItemType.ItemCategoryID == ItemCategoryID);
            if (ItemName != null)
                filtered = filtered.Where(x => x.ItemType.ItemTypeName == ItemName);
            if (ItemSizing != null)
                filtered = filtered.Where(x => x.ItemType.ItemSize == ItemSizing);
            if (ItemNumber != null)
                filtered = filtered.Where(x => x.ItemNumber == ItemNumber);
            if (ItemStatus != null)
                filtered = filtered.Where(x => x.ItemStatus == ItemStatus);
            return filtered.ToList();
        }
    }
}
