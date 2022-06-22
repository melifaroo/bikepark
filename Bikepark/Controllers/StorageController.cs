using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;
using Microsoft.Extensions.Options;

namespace Bikepark.Controllers
{
    [Authorize(Roles = "BikeparkManagers")]
    public class StorageController : Controller
    {
        private readonly BikeparkContext _context;
        private readonly IOptions<BikeparkConfig> _config;

        public StorageController(BikeparkContext context, IOptions<BikeparkConfig> config )
        {
            _context = context;
            _config = config;
        }

        // GET: Storage/Main
        public IActionResult Main()
        {
            return View();
        }

        // GET: Rental/Settings
        public IActionResult Settings()
        {
            return View(_config.Value);
        }


        // POST: Storage/SettingsUpdate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SettingsUpdate(BikeparkConfig config)
        {
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:WorkingHoursEnd", config.WorkingHoursEnd);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:WorkingHoursStart", config.WorkingHoursStart);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:MinServiceDelayBetweenRentsMinutes", config.MinServiceDelayBetweenRentsMinutes);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:DefaultRentTimeHours", config.DefaultRentTimeHours);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:GetBackWarningTimeMinutes", config.GetBackWarningTimeMinutes);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:ScheduleWarningTimeMinutes", config.ScheduleWarningTimeMinutes);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:OnServiceWarningTimeHours", config.OnServiceWarningTimeHours);
            BikeparkConfig.AddOrUpdateAppSetting("Bikepark:DefaultLogPageSize", config.DefaultLogPageSize); 

            _config.Value.WorkingHoursEnd = config.WorkingHoursEnd;
            _config.Value.WorkingHoursStart = config.WorkingHoursStart;
            _config.Value.MinServiceDelayBetweenRentsMinutes = config.MinServiceDelayBetweenRentsMinutes;
            _config.Value.DefaultRentTimeHours = config.DefaultRentTimeHours;
            _config.Value.GetBackWarningTimeMinutes = config.GetBackWarningTimeMinutes;
            _config.Value.ScheduleWarningTimeMinutes = config.ScheduleWarningTimeMinutes;
            _config.Value.OnServiceWarningTimeHours = config.OnServiceWarningTimeHours;
            _config.Value.DefaultLogPageSize = config.DefaultLogPageSize;
            
            return RedirectToAction("Settings");
        }

        // GET: Storage
        public async Task<IActionResult> Index()
        {
            //ARCHIVE
            ViewData["Items"] = _context.Items;//.ToListAsync();.Where(x => !x.Archival)
            return View(await _context.ItemTypes.ToListAsync());//.Where(x => !x.Archival)
        }

        // GET: Storage/InCategory/5
        public async Task<IActionResult> InCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Items"] = _context.Items;//.Where(x => !x.Archival)
            return View("Index", await _context.ItemTypes.Where(type => type.ItemCategoryID == id).ToListAsync());//.Where(x => !x.Archival)
        }

        // GET: Storage/WithPricingCategory/5
        public async Task<IActionResult> WithPricingCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Items"] = _context.Items;//.Where(x => !x.Archival)
            return View("Index", await _context.ItemTypes.Where(type => type.PricingCategoryID == id).ToListAsync());//.Where(x => !x.Archival)
        }

        // GET: Storage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.ItemTypeID == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // GET: Storage/Numbers
        public async Task<IActionResult> Numbers(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            var itemType = await _context.ItemTypes.FindAsync(id);
            var ItemsList = await _context.Items//.Where(x => !x.Archival)
                .Where(i => i.ItemTypeID == id)
                .ToListAsync();
            if (itemType == null)
            {
                return NotFound();
            }
            ViewData["ItemType"] = itemType;
            return View(new NumbersOfType { ItemsList = ItemsList, ItemTypeID = itemType.ItemTypeID });
        }

        // GET: Storage/Create
        public async Task<IActionResult> Create()
        {
            return await EditForm(new ItemType());
        }


        // GET: Storage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }
            return await EditForm(itemType);
        }

        private async Task<IActionResult> EditForm(ItemType itemType)
        {
            ViewData["ItemCategoryID"] = new SelectList(_context.ItemCategories, "ItemCategoryID", "ItemCategoryName", itemType.ItemCategoryID);
            ViewData["PricingCategoryID"] = new SelectList(_context.PricingCategories, "PricingCategoryID", "PricingCategoryName", itemType.PricingCategoryID);
            ViewData["ItemsCount"] = await _context.Items.Where(item => item.ItemTypeID == itemType.ItemTypeID).CountAsync();
            ViewData["ItemsArchivalCount"] = await _context.Items.IgnoreQueryFilters().Where(item => item.ItemTypeID == itemType.ItemTypeID).CountAsync();
            ViewData["HasRecords"] = itemType.ItemTypeID == null ? false : await _context.ItemRecords.AnyAsync(irecord => irecord.Item.ItemTypeID == itemType.ItemTypeID);
            return View("Edit", itemType);
        }

        // POST: Storage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemTypeID,ItemTypeName,ItemCategoryID,PricingCategoryID,ItemAge,ItemGender,ItemSize,ItemWheelSize,ItemColor,ItemDescription,ItemExternalURL,ItemImageURL")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                itemType.ItemTypeID = null;
                _context.Add(itemType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = itemType.ItemTypeID });
            }
            return await EditForm(itemType);
        }

        // POST: Storage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ItemTypeID,ItemTypeName,ItemCategoryID,PricingCategoryID,ItemAge,ItemGender,ItemSize,ItemWheelSize,ItemColor,ItemDescription,ItemExternalURL,ItemImageURL")] ItemType itemType)
        {
            var id = itemType.ItemTypeID;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.ItemTypeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = itemType.ItemTypeID });
            }
            return await EditForm(itemType);
        }

        // POST: Storage/Replace/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Replace([Bind("ItemTypeID,ItemTypeName,ItemCategoryID,PricingCategoryID,ItemAge,ItemGender,ItemSize,ItemWheelSize,ItemColor,ItemDescription,ItemExternalURL,ItemImageURL")] ItemType itemType)
        {
            var id = itemType.ItemTypeID;

            if (ModelState.IsValid)
            {
                ItemType replace = new ItemType
                {
                    ItemTypeName = itemType.ItemTypeName,
                    ItemCategoryID = itemType.ItemCategoryID,
                    PricingCategoryID = itemType.PricingCategoryID,
                    ItemAge = itemType.ItemAge,
                    ItemGender = itemType.ItemGender,
                    ItemSize = itemType.ItemSize,
                    ItemWheelSize = itemType.ItemWheelSize,
                    ItemColor = itemType.ItemColor,
                    ItemDescription = itemType.ItemDescription,
                    ItemExternalURL = itemType.ItemExternalURL
                };
                _context.Add(replace);
                await _context.SaveChangesAsync();

                var HasRecords = await _context.ItemRecords.AnyAsync(irecord => irecord.Item.ItemTypeID == id);
                if (HasRecords)
                {
                    LinqHelper.ForEach(_context.Items.Where(item => item.ItemTypeID == id), item => {
                        var hasRecords = _context.ItemRecords.Any(irecord => irecord.Item.ItemID == item.ItemID);
                        if (hasRecords)
                        {
                            item.Archival = true;
                            Item replaceItem = new Item
                            {
                                ItemNumber = item.ItemNumber,
                                ItemTypeID = replace.ItemTypeID
                            };
                            _context.Add(replaceItem);
                        }
                        else
                            item.ItemTypeID = replace.ItemTypeID;
                    }
                    );
                    itemType.Archival = true;
                }
                else
                {
                    LinqHelper.ForEach( _context.Items.Where(item => item.ItemTypeID == id), item => item.ItemTypeID = replace.ItemTypeID );
                    _context.ItemTypes.Remove(itemType);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = replace.ItemTypeID });
            }
            return await EditForm(itemType);
        }

        // POST: Storage/UpdateNumbers
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNumbers(NumbersOfType numbersOfType)
        {
            if (ModelState.IsValid)
            {
                if (numbersOfType.ItemTypeID == null)
                {
                    return NotFound();
                }

                //ARCHIVE
                var otherTypes = await _context.Items.AsNoTracking().Where(itemDB => itemDB.ItemTypeID != numbersOfType.ItemTypeID).ToListAsync();//.Where(x => !x.Archival)
                foreach (Item item in numbersOfType.ItemsList)
                { 
                    if (otherTypes.Any(itemDB => itemDB.ItemNumber == item.ItemNumber))
                    {
                        ViewData["Error"] = "номер используется " + item.ItemNumber;
                        return View("Numbers", numbersOfType);
                    }
                }

                foreach (Item itemDB in _context.Items.AsNoTracking().Where(itemDB => itemDB.ItemTypeID == numbersOfType.ItemTypeID))
                { // remove from record deleted items
                    if (!numbersOfType.ItemsList.Any(item => item.ItemID == itemDB.ItemID))
                    {
                        var hasRecords = _context.ItemRecords.Any(irecord => irecord.Item.ItemID == itemDB.ItemID);
                        if (hasRecords)
                        {
                            itemDB.Archival = true;
                            _context.Update(itemDB);
                        }
                        else
                            _context.Entry(itemDB).State = EntityState.Deleted;
                    }
                }
                foreach (Item item in numbersOfType.ItemsList)
                {
                    if (item.ItemID == null)
                    {
                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = numbersOfType.ItemTypeID });
            }

            return View("Numbers", numbersOfType);
        }

        // GET: Storage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //ARCHIVE
            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.ItemTypeID == id);
            if (itemType == null)
            {
                return NotFound();
            }

            ViewData["ItemsCount"] = await _context.Items.Where(item => item.ItemTypeID == id).CountAsync();
            ViewData["ItemsArchivalCount"] = await _context.Items.IgnoreQueryFilters().Where(item => item.ItemTypeID == id).CountAsync();
            ViewData["HasRecords"] = await _context.ItemRecords.AnyAsync(irecord => irecord.Item.ItemTypeID == id);
            return View(itemType);
        }

        // POST: Storage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //ARCHIVE
            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType != null)
            {
                var HasRecords = await _context.ItemRecords.AnyAsync(irecord => irecord.Item.ItemTypeID == id);
                if (HasRecords)
                {
                    LinqHelper.ForEach(_context.Items.Where(item => item.ItemTypeID == id), item => {
                        var hasRecords = _context.ItemRecords.Any(irecord => irecord.Item.ItemID == item.ItemID);
                        if (hasRecords)
                            item.Archival = true;
                        else
                            _context.Entry(item).State = EntityState.Deleted;
                    }
                    );
                    itemType.Archival = true;
                }
                else 
                { 
                    _context.Items.RemoveRange( _context.Items.Where(item => item.ItemTypeID == id) );
                    _context.ItemTypes.Remove(itemType);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemTypeExists(int? id)
        {
          return _context.ItemTypes.Any(e => e.ItemTypeID == id);
        }

        public async Task<PartialViewResult> ItemTypeNumbers(int TypeID)
        {
            //ARCHIVE
            return PartialView("_NumbersList", await _context.Items//.Where(x => !x.Archival)
                .Where(i => i.ItemTypeID == TypeID)
                .ToListAsync()  );
        }

        // GET: Storage/AddNumber
        public async Task<ActionResult> AddNumber(string ItemNumber, int TypeID)
        {
            //ARCHIVE
            if (await _context.Items.AnyAsync(item => item.ItemNumber == ItemNumber))//.Where(x => !x.Archival)
                return Json(new
                {
                    Status = 1,
                    Message = ItemNumber + " номер используется"
                });
            return PartialView("_Number", new Item { ItemNumber= ItemNumber, ItemTypeID = TypeID } );
        }

        // GET: Storage/AddNumber
        public async Task<ActionResult> AddNumberRange(string ItemNumberStart, string ItemNumberEnd, int TypeID)
        {
            //ARCHIVE
            if (int.TryParse(ItemNumberStart, out int start) && int.TryParse(ItemNumberEnd, out int end))
            {
                if (end - start > 10) { 
                    return Json(new
                    {
                        Status = 1,
                        Message = "не более 10 номеров за раз"
                    });                
                }
                var existedItems = await _context.Items.ToListAsync();//.Where(x => !x.Archival)
                existedItems = existedItems.Where(item => Convert.ToInt32(item.ItemNumber ?? "") >= start && Convert.ToInt32(item.ItemNumber ?? "") <= end).ToList();
                if (existedItems.Count>0)
                    return Json(new
                    {
                        Status = 1,
                        Message = String.Join(", ", existedItems.Select(item => item.ItemNumber).ToArray()) + " номера используется"
                    });
                var ItemsList = new List<Item>();
                    for (int i = start; i <= end; i++)
                        ItemsList.Add(new Item { ItemNumber = i.ToString(), ItemTypeID = TypeID });
                    return PartialView("_NumbersList", ItemsList);
            }
            else {
                return Json(new
                {
                    Status = 1,
                    Message = "Значения должны быть целочисленными"
                });            
            }
        }


        // GET: Rental/Contract
        public IActionResult Contract()
        {
            var model = new UploadFile();
            return View(model);
        }

        [HttpGet]
        public IActionResult ContractDownload()
        {
            var form = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, BikeparkConfig.FormsRelativePath, BikeparkConfig.ConractFormFileName));
            var fileName = BikeparkConfig.ConractFormFileName;
            byte[] fileBytes = System.IO.File.ReadAllBytes(form);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContractUpload(UploadFile model)
        {
            model.IsResponse = true;
            if (ModelState.IsValid)
            {
                try
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, BikeparkConfig.FormsRelativePath));

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    path = Path.Combine(path, BikeparkConfig.ConractFormFileName);
                    using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        await model.File.CopyToAsync(fileStream);
                    }


                    var warningMessage = ExcelTableHelper.ValidateContractForm(path);
                    model.IsSuccess = true;
                    model.HasWarnings = warningMessage.Length>0;
                    model.Message = "Форма успешно обновлена";
                    if (model.HasWarnings) model.Message = model.Message + "; В форме отсутствуют именованные диапазоны (ячейки): " + warningMessage;
                    return View("Contract", model);
                }
                catch
                {
                    model.Message = "Не удалось загрузить форму";
                    return View("Contract", model);
                }
            }
            model.Message = "Не удалось загрузить форму";
            return View("Contract", model);

        }

        public async Task<FileResult> Export()
        {
            var fileName = "Storage";
            var folderPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\Docs\Temp"));
            var storage = await _context.Items.ToListAsync();
            var data = storage//.Include(irecord => irecord.Item).ThenInclude(item => item.ItemType).ThenInclude(type => type.ItemCategory).Include(irecord => irecord.Pricing)
                .Select(item => new StorageExportModel
                {
                    ItemID = item.ItemID,
                    ItemNumber = item.ItemNumber,
                    ItemTypeName = item.ItemType?.ItemTypeName,
                    ItemCategoryName = item.ItemType?.ItemCategory?.ItemCategoryName,
                    PricingCategoryName = item.ItemType?.PricingCategory?.PricingCategoryName,
                    ItemAge = item.ItemType == null ? null : item.ItemType.ItemAge == null ? null : EnumHelper<ItemAge>.GetDisplayValue((ItemAge)(item.ItemType.ItemAge)),
                    ItemGender = item.ItemType == null ? null : item.ItemType.ItemGender == null ? null : EnumHelper<ItemGender>.GetDisplayValue((ItemGender)(item.ItemType.ItemGender)),
                    ItemSize = item.ItemType == null ? null : item.ItemType.ItemSize == null ? null : EnumHelper<ItemSize>.GetDisplayValue((ItemSize)(item.ItemType.ItemSize)),
                    ItemWheelSize = item.ItemType?.ItemWheelSize,
                    ItemColor = item.ItemType?.ItemColor,
                    ItemDescription = item.ItemType?.ItemDescription,
                    ItemExternalURL = item.ItemType?.ItemExternalURL,
                })
                .ToList();

            var (fileFullName, fileNameWithExt) = ExcelTableHelper.CreateExcelFile<StorageExportModel>(data, folderPath, fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileFullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameWithExt);
        }


    }
}
