using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;
using System.Text;
using Microsoft.Extensions.Options;

namespace Bikepark.Controllers
{
    public class LogController : Controller
    {
        private readonly BikeparkContext _context;
        private readonly IOptions<BikeparkConfig> _config;
        private readonly Status[] AllStatuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToArray();

        public LogController(BikeparkContext context, IOptions<BikeparkConfig> config)
        {
            _context = context;
            _config = config;
        }

        private IActionResult Log(IEnumerable<ItemRecord> log, string logName, Status[]? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            ViewData["LogName"] = logName;
            ViewData["Statuses"] = statuses != null?statuses.Cast<int>():null;
            ViewData["From"] = from;
            ViewData["To"] = to;
            return View("Index", log);
        }

        private int statusOrder(Status status, DateTime? start, DateTime? end)
        {
            return (status == Status.Active && end < DateTime.Now) ? 0 :
                    (status == Status.Scheduled && start < DateTime.Now) ? 1 :
                    (status == Status.Service) ? 2 :
                    (status == Status.OnService && end < DateTime.Now) ? 3 :
                    (status == Status.Active && end < DateTime.Now.AddMinutes(_config.Value.ScheduleWarningTimeMinutes)) ? 4 :
                    (status == Status.Scheduled && start < DateTime.Now.AddMinutes(_config.Value.ScheduleWarningTimeMinutes)) ? 5 :
                    (status == Status.OnService && start < DateTime.Now.AddHours(_config.Value.OnServiceWarningTimeHours)) ? 6 :
                    (status == Status.Draft) ? 7 :
                    (status == Status.Active) ? 8 :
                    (status == Status.Scheduled) ? 9 :
                    (status == Status.OnService) ? 10 : 11;
        }

        // GET: Log
        public async Task<IActionResult> Index(string? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            statuses = (statuses == null || statuses.Length==0) ? (string.Join(",", AllStatuses.Cast<int>())) : statuses;
            var Statuses = statuses.Split(",").Select(Int32.Parse).Cast<Status>().ToArray();
            var name = "Записи";
            await _context.ItemRecords
                .Where(irecord => Statuses.Contains(irecord.Status) )
                .Where(irecord => (to == null || irecord.Start <= ((DateTime)to).AddDays(1)) && (from == null || irecord.Start >= ((DateTime)from)))
                .ForEachAsync(irecord => irecord.Attention(_config.Value.GetBackWarningTimeMinutes, _config.Value.ScheduleWarningTimeMinutes, _config.Value.OnServiceWarningTimeHours));
            await _context.SaveChangesAsync();
            var log = await _context.ItemRecords
                .Where(irecord => Statuses.Contains(irecord.Status) )
                .Where(irecord => (to == null || irecord.Start <= ((DateTime)to).AddDays(1)) && (from == null || irecord.Start >= ((DateTime)from)))
                .OrderBy(irecord => irecord.AttentionStatus)
                .ThenByDescending(irecord => irecord.End)
                .ToListAsync();
            DateTime? oldest = null;
            DateTime? newest = null;
            if (log.Count > 0)
            {
                oldest = log.LastOrDefault()?.Start;
                newest = log.FirstOrDefault()?.End;
            }
            return Log( log, name, Statuses, from ?? oldest, to ?? newest );
        }

        // POST: Log
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter( [FromForm] DateTime From, DateTime To, bool Active, bool Scheduled, bool Closed, bool Draft, bool Service, bool OnService, bool Fixed)
        {
            var statuses = new List<Status>();
            if (Active) statuses.Add(Status.Active);
            if (Scheduled) statuses.Add(Status.Scheduled);
            if (Closed) statuses.Add(Status.Closed);
            if (Draft) statuses.Add(Status.Draft);
            if (Service) statuses.Add(Status.Service);
            if (OnService) statuses.Add(Status.OnService);
            if (Fixed) statuses.Add(Status.Fixed);

            return RedirectToAction( nameof(Index), new { statuses = string.Join(",", statuses.Cast<int>()), from = From, to = To } );
        }


        // GET: Log/OfType/5
        public async Task<IActionResult> OfType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var type = await _context.ItemTypes.IgnoreQueryFilters().FirstOrDefaultAsync(type => type.ItemTypeID == id);
            if (type == null)
            {
                return NotFound();
            }
            var name = type.ItemTypeName;
            var log = await _context.ItemRecords
                .Where(irecord => irecord.Item.ItemTypeID == id)
                .OrderByDescending(irecord => irecord.End)
                .ToListAsync();
            return Log(log, "Записи по модели " + name +" (#"+id+")" );
        }

        // GET: Log/OfItem/5
        public async Task<IActionResult> OfItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await _context.Items.IgnoreQueryFilters().FirstOrDefaultAsync(item => item.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }
            var number = item.ItemNumber;
            var log = await _context.ItemRecords
                .Where(irecord => irecord.ItemID == id)
                .OrderByDescending(record => record.End)
                .ToListAsync();
            return Log(log, "Записи по номеру #" + number );
        }

        // GET: Log/WithPricing/5
        public async Task<IActionResult> WithPricing(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pricing = await _context.Pricings.IgnoreQueryFilters().FirstOrDefaultAsync(pricing => pricing.PricingID == id);
            if (pricing == null)
            {
                return NotFound();
            }
            var name = pricing.PricingName;
            var log = await _context.ItemRecords
                .Where(irecord => irecord.PricingID == id)
                .OrderByDescending(record => record.End)
                .ToListAsync();
            return Log(log, "Записи по тарифу #" + name );
        }


        // GET: Log/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemNumber");
            ViewData["PricingID"] = new SelectList(_context.Pricings, "PricingID", "PricingID");
            ViewData["RecordID"] = new SelectList(_context.Records, "RecordID", "RecordID");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Log/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemRecordID,RecordID,ItemID,PricingID,Status,Start,End,CustomInformation,UserID")] ItemRecord itemRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemNumber", itemRecord.ItemID);
            ViewData["PricingID"] = new SelectList(_context.Pricings, "PricingID", "PricingID", itemRecord.PricingID);
            ViewData["RecordID"] = new SelectList(_context.Records, "RecordID", "RecordID", itemRecord.RecordID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", itemRecord.UserID);
            return View(itemRecord);
        }

        // GET: Log/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ItemRecords == null)
            {
                return NotFound();
            }

            var itemRecord = await _context.ItemRecords.FindAsync(id);
            if (itemRecord == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemNumber", itemRecord.ItemID);
            ViewData["PricingID"] = new SelectList(_context.Pricings, "PricingID", "PricingID", itemRecord.PricingID);
            ViewData["RecordID"] = new SelectList(_context.Records, "RecordID", "RecordID", itemRecord.RecordID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", itemRecord.UserID);
            return View(itemRecord);
        }

        // POST: Log/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("ItemRecordID,RecordID,ItemID,PricingID,Status,Start,End,CustomInformation,UserID")] ItemRecord itemRecord)
        {
            if (id != itemRecord.ItemRecordID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemRecordExists(itemRecord.ItemRecordID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemNumber", itemRecord.ItemID);
            ViewData["PricingID"] = new SelectList(_context.Pricings, "PricingID", "PricingID", itemRecord.PricingID);
            ViewData["RecordID"] = new SelectList(_context.Records, "RecordID", "RecordID", itemRecord.RecordID);
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Id", itemRecord.UserID);
            return View(itemRecord);
        }

        private bool ItemRecordExists(int? id)
        {
          return (_context.ItemRecords?.Any(e => e.ItemRecordID == id)).GetValueOrDefault();
        }

        // GET: Log/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ItemRecords == null)
            {
                return NotFound();
            }

            var itemRecord = await _context.ItemRecords
                .Include(i => i.Item)
                .Include(i => i.Pricing)
                .Include(i => i.Record)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ItemRecordID == id);
            if (itemRecord == null)
            {
                return NotFound();
            }

            return View(itemRecord);
        }

        // POST: Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.ItemRecords == null)
            {
                return Problem("Entity set 'BikeparkContext.ItemRecords'  is null.");
            }
            var itemRecord = await _context.ItemRecords.FindAsync(id);
            if (itemRecord != null)
            {
                _context.ItemRecords.Remove(itemRecord);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DeleteItemRecord(int? id)
        {
            Console.WriteLine(Request.RouteValues);
            if (id == null)
            {
                return NotFound();
            }
            var rentalRecord = await _context.ItemRecords.FirstOrDefaultAsync(m => m.ItemRecordID == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }
            return View(rentalRecord);
        }

        // POST: Rental/Delete/5
        [HttpPost, ActionName("DeleteItemRecord")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItemRecordConfirmed(int id)
        {
            var rentalRecord = await _context.ItemRecords.FindAsync(id);
            _context.ItemRecords.Remove(rentalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Log/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ItemRecords == null)
            {
                return NotFound();
            }

            var itemRecord = await _context.ItemRecords
                .Include(i => i.Item)
                .Include(i => i.Pricing)
                .Include(i => i.Record)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.ItemRecordID == id);
            if (itemRecord == null)
            {
                return NotFound();
            }

            return View(itemRecord);
        }

        // GET: Rental/ItemDetails/5
        public async Task<IActionResult> ItemRentalDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.IgnoreQueryFilters()
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["ItemRecs"] = await _context.ItemRecords
                .Where(r => r.Status != Status.Draft && r.Status != Status.Closed && r.Status != Status.Fixed)
                .Where(r => r.ItemID == id)
                .Include(r => r.Record)
                .OrderBy(r => r.Status)
                .ToListAsync();
            return View(item);
        }


        internal async Task<IActionResult> ControlService(IEnumerable<ItemRecord> itemRecords)
        {
            ViewData["Pricings"] = _context.Pricings;
            ViewData["ArchivalPricings"] = await _context.Pricings.IgnoreQueryFilters().ToListAsync();
            ViewData["Availability"] = _context.GetAvailability(null, 0);
            return View("ControlService", itemRecords);
        }

        // GET: Log/ControlRecordService/5
        public async Task<IActionResult> ControlRecordService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<ItemRecord> itemRecords = await _context.ItemRecords.Where(ir => ir.RecordID == id && ir.Status > Status.Closed).ToListAsync();
            if (itemRecords == null || itemRecords.Count == 0)
            {
                return NotFound();
            }

            ViewData["RecordID"] = id;
            return await ControlService(itemRecords);
        }

        // GET: Log/ControlItemService/5
        public async Task<IActionResult> ControlItemService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ItemRecord itemRecord = await _context.ItemRecords.FindAsync(id);

            if (itemRecord == null)
            {
                return NotFound();
            }
            if (itemRecord.Status < Status.Service)
            {
                return NotFound();
            }
            return await ControlService(new List<ItemRecord>() { itemRecord });
        }

        // GET: Log/Service
        public IActionResult Service()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ControlNumberService(Number number)
        {
            if (ModelState.IsValid)
            {
                Item item = await _context.Items.FirstOrDefaultAsync(item => item.ItemNumber == number.ItemNumber);
                if (item == null)
                {
                    ViewData["Error"] = "номер не найден";
                    return View("Service", number);
                }
                return await ControlService(new List<ItemRecord>() { new ItemRecord { ItemID = item.ItemID, Item = item, Start = DateTime.Now, End = DateTime.Now.AddDays(1), Status = Status.Service } });
            }
            return View("Service", number);
        }

        public async Task<PartialViewResult> AddServiceItemRecord(int ItemID, DateTime Start, DateTime End, int? RecordID)
        {
            var Item = await _context.Items.FindAsync(ItemID);
            ViewData["Pricings"] = _context.Pricings;
            return PartialView("_ItemRecordRow_service", new ItemRecord { ItemID = ItemID, Item = Item, Start = Start, End = End, Status = Status.Service, RecordID = RecordID });
        }

        // POST: Rental/UpdateService
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateService(IEnumerable<ItemRecord> itemRecords)
        {
            if (ModelState.IsValid)
            {
                //if (!CheckServiceAvailability(itemRecords))
                //{
                //    ViewData["Error"] = "Некоторые позиции забронированы на указанное время ремонта";
                //    return await ControlService(itemRecords);
                //}
                foreach (ItemRecord itemRecord in itemRecords)
                {
                    if (itemRecord.ItemRecordID == null && itemRecord.ItemID != null)
                    {
                        _context.Add(itemRecord);
                    }
                    else
                    {
                        if (itemRecord.ItemID == null)
                        {
                            _context.Remove(itemRecord);
                        }
                        else
                        {
                            _context.Update(itemRecord);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index), new { statuses = string.Join(",", (new Status[] { Status.Service, Status.OnService }).Cast<int>())  });
            }
            else
            {
                return await ControlService(itemRecords);
            }
        }



        // GET: Rental/ControlItemService/5
        public async Task<IActionResult> UpdateServiceFixed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ItemRecord irec = await _context.ItemRecords.FindAsync(id);

            if (irec == null)
            {
                return NotFound();
            }
            if (irec.Status != Status.OnService)
            {
                return NotFound();
                //return RedirectToAction(nameof(ServiceIndex));
            }
            irec.Status = Status.Fixed;
            _context.Update(irec);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { statuses = string.Join(",", (new Status[] { Status.Service, Status.OnService }).Cast<int>()) });
        }

        public async Task<FileResult> Export(string? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            var fileName = "Log";
            var folderPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\Docs\Temp"));    

            statuses = (statuses == null || statuses.Length == 0) ? (string.Join(",", AllStatuses.Cast<int>())) : statuses;
            var Statuses = statuses.Split(",").Select(Int32.Parse).Cast<Status>().ToArray();
            var data = await _context.ItemRecords
                .Where(irecord => Statuses.Contains(irecord.Status))
                .Where(irecord => (to == null || irecord.Start <= ((DateTime)to).AddDays(1)) && (from == null || irecord.Start >= ((DateTime)from)))
                .OrderByDescending(irecord => irecord.End)
                .Select(irecord => new ItemRecordExportModel { 
                    ItemRecordID = irecord.ItemRecordID,
                    ItemCategoryName = irecord.Item.ItemType.ItemCategory.ItemCategoryName,
                    ItemTypeName = irecord.Item.ItemType.ItemTypeName,
                    ItemNumber = irecord.Item.ItemNumber,
                    Start = irecord.Start,
                    End = irecord.End,
                    Status = EnumHelper<Status>.GetDisplayValue(irecord.Status),
                    PricingName = irecord.Pricing.PricingName,
                    Price = irecord.Pricing.Price,
                    PricingType = EnumHelper<PricingType>.GetDisplayValue(irecord.Pricing.PricingType),
                    RecordID = irecord.RecordID
                })
                .ToListAsync();

            var (fileFullName, fileNameWithExt) = ExcelTableHelper.CreateExcelFile<ItemRecordExportModel>(data, folderPath, fileName);

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileFullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameWithExt);
        }

    }
}
