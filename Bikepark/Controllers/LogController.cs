using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;

namespace Bikepark.Controllers
{
    public class LogController : Controller
    {
        private readonly BikeparkContext _context;

        public LogController(BikeparkContext context)
        {
            _context = context;
        }

        private IActionResult Log(IEnumerable<ItemRecord> log, string logName, bool rental = true, bool service = false, bool actual = false, DateTime? from = null, DateTime? to = null)
        {
            ViewData["LogName"] = logName;
            ViewData["Service"] = service;
            ViewData["Rental"] = rental;
            ViewData["Actual"] = actual;
            ViewData["From"] = from;
            ViewData["To"] = to;
            return View("Index", log);
        }

        // GET: Log
        public async Task<IActionResult> Index()
        {
            var log = await _context.ItemRecords.OrderByDescending(irecord => irecord.End).ToListAsync();//.Include(i => i.Item).Include(i => i.Pricing).Include(i => i.Record).Include(i => i.User);
            return Log( log, "Все записи" );
        }

        // GET: Log/Service
        public async Task<IActionResult> Service(bool Actual = false, DateTime? from = null, DateTime? to = null)
        {
            var earlyStatus = Actual ? Status.OnService : Status.Service;
            var lateStatus  = Actual ? Status.OnService : Status.Fixed;
            var log = await _context.ItemRecords.Where(irecord => irecord.Status >= earlyStatus && irecord.Status <= lateStatus).OrderByDescending(irecord => irecord.End).ToListAsync();
            return Log(log, "Записи о ремонте", false, true, Actual, from, to);
        }

        // GET: Log/Actual
        public async Task<IActionResult> Rental(bool Actual = false, DateTime? from = null, DateTime? to = null)
        {
            var earlyStatus = Actual ? Status.Scheduled : Status.Draft;
            var lateStatus  = Actual ? Status.Active : Status.Closed;
            var log = await _context.ItemRecords.Where(irecord => irecord.Status >= earlyStatus && irecord.Status <= lateStatus).OrderByDescending(irecord => irecord.End).ToListAsync();
            return Log(log, "Записи о прокате", true, false, Actual, from, to);
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
            var log = await _context.ItemRecords.Where(irecord => irecord.Item.ItemTypeID == id).OrderByDescending(irecord => irecord.End).ToListAsync();
            return Log(log, "Записи по модели " + name +" (#"+id+")", false, false );
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
            var log = await _context.ItemRecords.Where(irecord => irecord.ItemID == id).OrderByDescending(record => record.End).ToListAsync();
            return Log(log, "Записи по номеру #" + number, false, false );
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
            var log = await _context.ItemRecords.Where(irecord => irecord.PricingID == id).OrderByDescending(record => record.End).ToListAsync();
            return Log(log, "Записи по тарифу #" + name, false, false );
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

        // GET: Rental/ControlItemService/5
        public async Task<IActionResult> ControlService(int? id)
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

        // GET: Rental/NumberOnService
        public IActionResult ServiceForNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ControlServiceForNumber(Number number)
        {
            if (ModelState.IsValid)
            {
                Item item = await _context.Items.FirstOrDefaultAsync(item => item.ItemNumber == number.ItemNumber);
                if (item == null)
                {
                    ViewData["Error"] = "номер не найден";
                    return View("NumberOnService", number);
                }
                return await ControlService(new List<ItemRecord>() { new ItemRecord { ItemID = item.ItemID, Item = item, Start = DateTime.Now, End = DateTime.Now.AddDays(1), Status = Status.Service } });
            }
            return View("NumberOnService", number);
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
                return RedirectToAction(nameof(LogController.Service), nameof(LogController));
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
            return RedirectToAction(nameof(LogController.Service), nameof(LogController));
        }
    }
}
