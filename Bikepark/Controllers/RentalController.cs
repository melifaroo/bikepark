using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Bikepark.Controllers
{

    [Authorize(Roles = "BikeparkAdministrators,BikeparkManagers")]
    public class RentalController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BikeparkContext _context;
        private readonly IOptions<BikeparkConfig> _config;
        private readonly Status[] AllStatuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToArray();

        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }

        public RentalController(BikeparkContext context, IOptions<BikeparkConfig> config, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _config = config;
        }

        // GET: Rental/Main
        public IActionResult Main()
        {
            return View();
        }

        // GET: Rental/Prepare
        public async Task<IActionResult> Prepare()
        {
            ViewData["Items"] = _context.Items;
            ViewData["Types"] = await _context.ItemTypes.ToListAsync();
            return View(await _context.Prepared.Select(prep => prep.Item).ToListAsync());
        }

        // GET: Rental/Pricings
        public async Task<IActionResult> Pricings()
        {
            return View(await _context.Pricings.ToListAsync());
        }

        // GET: Rental/Settings
        public IActionResult Settings()
        {
            return View();
        }

        // GET: Rental/stat
        public async Task<IActionResult> Stat()
        {
            return Json(new
            {
                Scheduled = await _context.Records.Where(record => record.Status == Status.Scheduled).CountAsync(),
                ScheduledWarning = await _context.Records.Where(record => record.Status == Status.Scheduled).Where(record => record.Start < DateTime.Now.AddMinutes(_config.Value.ScheduleWarningTimeMinutes) ).CountAsync(),
                ScheduledOverdue = await _context.Records.Where(record => record.Status == Status.Scheduled).Where(record => record.Start < DateTime.Now).CountAsync(),
                Active = await _context.Records.Where(record => record.Status == Status.Active).CountAsync(),
                ActiveWarning = await _context.Records.Where(record => record.Status == Status.Active).Where(record => record.End < DateTime.Now.AddMinutes(_config.Value.GetBackWarningTimeMinutes)).CountAsync(),
                ActiveOverdue = await _context.Records.Where(record => record.Status == Status.Active).Where(record => record.End < DateTime.Now).CountAsync(),
                Service = await _context.ItemRecords.Where(record => record.Status >= Status.Service && record.Status < Status.Fixed).CountAsync(),
                ServiceWarning = await _context.ItemRecords.Where(record => record.Status == Status.OnService).Where(record => record.End < DateTime.Now.AddHours(_config.Value.OnServiceWarningTimeHours)).CountAsync(),
                ServiceNeedAction = await _context.ItemRecords.Where(record => record.Status == Status.Service || (record.Status == Status.OnService && record.End < DateTime.Now )).CountAsync(),
            });
        }

        private IActionResult Log(IEnumerable<Record> log, string logName, Status[]? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            statuses = (statuses == null || statuses.Length == 0) ? AllStatuses : statuses;
            ViewData["LogName"] = logName;
            ViewData["Statuses"] = statuses;
            ViewData["From"] = from;
            ViewData["To"] = to;
            return View("Index", log);
        }

        // GET: Rental
        public async Task<IActionResult> Index(string? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            statuses = (statuses == null || statuses.Length == 0) ? (string.Join(",", AllStatuses.Cast<int>())) : statuses;
            var Statuses = statuses.Split(",").Select(Int32.Parse).Cast<Status>().ToArray();
            var name = "Заказы" ;
            await _context.Records
                .Where(record => Statuses.Contains(record.Status))
                .Where(record => (to == null || record.Start <= ((DateTime)to).AddDays(1)) && (from == null || record.Start >= ((DateTime)from)))
                .ForEachAsync( record => record.Attention(_config.Value.GetBackWarningTimeMinutes, _config.Value.ScheduleWarningTimeMinutes, _config.Value.OnServiceWarningTimeHours) );
            await _context.SaveChangesAsync();
            var log = await _context.Records
                .Where(record => Statuses.Contains(record.Status))
                .Where(record => (to == null || record.Start <= ((DateTime)to).AddDays(1)) && (from == null || record.Start >= ((DateTime)from)))
                .OrderBy(record => record.AttentionStatus)
                .ThenByDescending(record => record.Start)
                .ToListAsync();
            DateTime? oldest = null ;
            DateTime? newest = null;
            if (log.Count > 0)
            {
                oldest = log.LastOrDefault()?.Start;
                newest = log.FirstOrDefault()?.End;
            }
            return Log(log, name, Statuses, from ?? oldest, to ?? newest);
        }

        // POST: Rental
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter( [FromForm] DateTime From, DateTime To, bool Active, bool Scheduled, bool Closed, bool Draft)
        {
            var statuses = new List<Status>();
            if (Active) statuses.Add(Status.Active); 
            if (Scheduled) statuses.Add(Status.Scheduled);
            if (Closed) statuses.Add(Status.Closed);
            if (Draft) statuses.Add(Status.Draft);

            return RedirectToAction(nameof(Index), new { statuses = string.Join(",", statuses.Cast<int>()), from = From, to = To });
        }

        // GET: Rental/Create
        public async Task<IActionResult> Create()
        {
            return await Control(new Record());
        }

        // GET: Rental/Control/5
        public async Task<IActionResult> Control(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.Records.IgnoreQueryFilters()
                    .Include(r => r.ItemRecords).ThenInclude(r => r.Item).ThenInclude(r => r.ItemType)
                    .Include(r => r.ItemRecords).ThenInclude(r => r.Pricing)
                    .FirstOrDefaultAsync(ir => ir.RecordID == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }
            return await Control(rentalRecord);
        }

        private async Task<IActionResult> Control(Record rentalRecord)
        {
            ViewData["Items"] = _context.Items;
            ViewData["Types"] = await _context.ItemTypes.ToListAsync();
            ViewData["Pricings"] = _context.Pricings;
            ViewData["Holidays"] = await _context.Holidays.Select(day => day.Date).ToListAsync();
            ViewData["Prepared"] = await _context.Prepared.Select(prep=> prep.ItemID).ToListAsync();

            ViewData["ArchivalPricings"] = await _context.Pricings.IgnoreQueryFilters().ToListAsync();
            ViewData["Availability"] = _context.GetAvailability(rentalRecord.RecordID, _config.Value.MinServiceDelayBetweenRentsMinutes);

            ViewData["WorkingHoursStart"] = _config.Value.WorkingHoursStart;
            ViewData["WorkingHoursEnd"] = _config.Value.WorkingHoursEnd;
            ViewData["MinServiceDelayBetweenRentsMinutes"] = _config.Value.MinServiceDelayBetweenRentsMinutes;
            ViewData["DefaultRentTimeHours"] = _config.Value.DefaultRentTimeHours;

            foreach (var irec in rentalRecord.ItemRecords)
                if (irec.Item == null && irec.ItemID != null)
                    irec.Item = await _context.Items.IgnoreQueryFilters().FirstOrDefaultAsync(ir => ir.ItemID == irec.ItemID);

            return View("Control", rentalRecord);
        }

        public bool CheckAvailability(Record rentalRecord, Status Action, DateTime ActionTime) {
            var availability = _context.GetAvailability( rentalRecord.RecordID, _config.Value.MinServiceDelayBetweenRentsMinutes ) ;
            var record = new ItemRecord {
                Start = (rentalRecord.Status < Status.Active ? rentalRecord.Start : ActionTime) ?? DateTime.Now,
                End = rentalRecord.End ?? DateTime.Now,
                Status = Status.Draft
            };
            foreach (ItemRecord itemRecord in rentalRecord.ItemRecords.Where(ir => ir.Status < Status.Active)) {
                if (itemRecord.ItemID!=null && availability.ContainsKey( itemRecord.ItemID??-1 )) {
                    if (Overlap(availability[itemRecord.ItemID ?? -1], record, itemRecord.ItemRecordID ) )
                        return false;
                }
            }
            return true;            
        }
        public bool Overlap(List<ItemRecord> periods, ItemRecord record2, int? itemRecordID)
        {
            foreach (var record1 in periods)
            {
                if (RecordInfo.Overlap(record1, record2) && record1.ItemRecordID != itemRecordID)
                    return true;
            }
            return false;
        }

        // POST: Rental/UpdateOrCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrCreate(Record rentalRecord)
        {
            if (ModelState.IsValid)
            {
                Status Action = (Status)Enum.Parse(typeof(Status), Request.Form["StatusAction"]);
                DateTime ActionTime = DateTime.Parse(Request.Form["TimeAction"]);
                if (CheckAvailability(rentalRecord, Action, ActionTime))
                {

                    var user = await _userManager.GetUserAsync(User);
                    if (Request.Form["CustomerForceCreate"] == "on")
                    {
                        rentalRecord.Customer.CustomerID = null;
                    }

                    if (rentalRecord.Customer.CustomerID != null)
                    {
                        rentalRecord.CustomerID = rentalRecord.Customer.CustomerID;
                        if (Request.Form["CustomerForceUpdate"] == "on")
                        {
                            _context.Update(rentalRecord.Customer);
                        }
                        rentalRecord.Customer = null;
                    }
                    var toFix = new List<ItemRecord>();
                    if (rentalRecord.RecordID == null) //Create new draft record
                    {
                        rentalRecord.Status = Action;
                        foreach (ItemRecord rentalItem in rentalRecord.ItemRecords)
                        {
                            rentalItem.Start = rentalRecord.Start;
                            rentalItem.End = rentalRecord.End;
                            rentalItem.Record = rentalRecord;
                            rentalItem.Status = rentalRecord.Status;
                        }
                        rentalRecord.Price = await ComputePrice(rentalRecord);
                        _context.Add(rentalRecord);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        try
                        {
                            rentalRecord.Status = Action;
                            foreach (ItemRecord itemRecordDB in _context.ItemRecords.AsNoTracking().Where(itrec => itrec.RecordID == rentalRecord.RecordID))
                            { // remove from record deleted items
                                if (!rentalRecord.ItemRecords.Any(ItemRecord => ItemRecord.ItemRecordID == itemRecordDB.ItemRecordID))
                                    _context.ItemRecords.Remove(itemRecordDB);
                            }
                            int closeditems = 0;
                            DateTime CloseTime = DateTime.MinValue;
                            foreach (ItemRecord itemRecord in rentalRecord.ItemRecords)
                            {
                                itemRecord.RecordID = rentalRecord.RecordID;
                                itemRecord.UserID = user.Id;
                                if (rentalRecord.Status < Status.Active) // activate all
                                {
                                    itemRecord.Start = rentalRecord.Start;
                                    itemRecord.End = rentalRecord.End;
                                    itemRecord.Status = rentalRecord.Status;
                                }
                                else if (rentalRecord.Status == Status.Active)
                                {
                                    var itemRecordDB = _context.ItemRecords.AsNoTracking().FirstOrDefault(itrec => itrec.ItemRecordID == itemRecord.ItemRecordID && itrec.Status == Status.Closed);
                                    if (itemRecordDB != null) //already closed
                                    {
                                        closeditems++;
                                        CloseTime = DateTimeX.Max(CloseTime, itemRecordDB.End ?? CloseTime);
                                        continue;
                                    }
                                    if (itemRecord.Status <= Status.Active) // active or to activate
                                    {
                                        itemRecord.Start = itemRecord.Status < Status.Active ? ActionTime : itemRecord.Start;
                                        itemRecord.End = rentalRecord.End;
                                        itemRecord.Status = Status.Active;
                                    }
                                    else
                                    if (itemRecord.Status == Status.Closed) // to close
                                    {
                                        itemRecord.Start = itemRecord.Start;
                                        itemRecord.End = ActionTime;
                                        itemRecord.Status = Status.Closed;
                                        closeditems++;
                                        CloseTime = DateTimeX.Max(CloseTime, ActionTime);
                                    }
                                    else
                                    if (itemRecord.Status == Status.Service) // to close
                                    {
                                        itemRecord.Start = itemRecord.Start;
                                        itemRecord.End = ActionTime;
                                        itemRecord.Status = Status.Closed;
                                        closeditems++;
                                        CloseTime = DateTimeX.Max(CloseTime, ActionTime);
                                        toFix.Add(new ItemRecord { ItemID = itemRecord.ItemID, Item = itemRecord.Item, Status = Status.Service, RecordID = itemRecord.RecordID, UserID = user.Id });
                                    }
                                }
                                //_context.Update(itemRecord);
                            }
                            if (closeditems == rentalRecord.ItemRecords.Count && rentalRecord.Status == Status.Active)
                            { // close record
                                rentalRecord.Status = Status.Closed;
                                rentalRecord.End = CloseTime;
                            }

                            rentalRecord.Price = await ComputePrice(rentalRecord);


                            if (toFix.Count > 0)
                                _context.AddRange(toFix);

                            _context.Update(rentalRecord);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!RentalRecordExists((int)rentalRecord.RecordID))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    return (toFix.Count > 0) ?
                        RedirectToAction(nameof(LogController.ControlRecordService), "Log", new { id = rentalRecord.RecordID }) :
                        RedirectToAction(nameof(Control), new { id = rentalRecord.RecordID });
                }
            }

            if ((rentalRecord.RecordID ?? 0) <= 0)
            {
                return await Control(rentalRecord);
            }
            else
            {
                return RedirectToAction(nameof(Control), new { id = rentalRecord.RecordID });
            }

        }

        // POST: Rental/UpdateOrCreate
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contract(Record rentalRecord)
        {
            if (ModelState.IsValid)
            {
                Status Action = (Status)Enum.Parse(typeof(Status), Request.Form["StatusAction"]);
                DateTime ActionTime = DateTime.Parse(Request.Form["TimeAction"]);
                if (CheckAvailability(rentalRecord, Action, ActionTime))
                {

                    var form = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\Docs\Forms", "RentalContractForm.xlsx"));
                    var fileName = "Contract";
                    var folderPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\Docs\Temp"));

                    foreach (var irec in rentalRecord.ItemRecords)
                        if (irec.Item == null && irec.ItemID != null)
                            irec.Item = await _context.Items.IgnoreQueryFilters().FirstOrDefaultAsync(ir => ir.ItemID == irec.ItemID);

                    rentalRecord.Price = await ComputePrice(rentalRecord);

                    var (fileFullName, fileNameWithExt) = ExcelTableHelper.UpdateContractForRecord(rentalRecord, form, folderPath, fileName);

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileFullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameWithExt);
                }
            }

            if ((rentalRecord.RecordID ?? 0) <= 0)
            {
                return await Control(rentalRecord);
            }
            else
            {
                return RedirectToAction(nameof(Control), new { id = rentalRecord.RecordID });
            }
        }

        // POST: Rental/SaveServiceRecords
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrepare(IEnumerable<Item> items)
        {
            if (ModelState.IsValid)
            {
                LinqHelper.ForEach(_context.Prepared, prepared => _context.Entry(prepared).State = EntityState.Deleted);
                foreach (Item item in items)
                {                    
                    if (item.ItemID != null)
                    {
                        _context.Add(new ItemPrepared { ItemID = (int)item.ItemID } );
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Prepare));
            }
            else
            {
                return RedirectToAction(nameof(Prepare));
            }
        }

        private async Task<double> ComputePrice(Record rentalRecord)
        {
            double price = 0;
            foreach (ItemRecord irec in rentalRecord.ItemRecords) {
                var Pricing = await _context.Pricings.FindAsync(irec.PricingID);
                price += Pricing.Price * (Pricing.PricingType == PricingType.Hourly ? (irec.End - irec.Start).GetValueOrDefault().Hours : 1);
            }
            return price;
        }


        // GET: Rental/GetBack
        public IActionResult GetBack()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Control(Number number) {

            if (ModelState.IsValid)
            {
                var itemRec = await _context.ItemRecords.FirstOrDefaultAsync(irec => irec.Item.ItemNumber == number.ItemNumber && irec.Status == Status.Active);
                if (itemRec == null || itemRec.Record==null) {
                    ViewData["Error"] = "запись не найдена";
                    return View("GetBack", number);
                }
                
                return RedirectToAction(nameof(Control), new { id = itemRec.Record.RecordID });
            }
            return View("GetBack", number);
        }

        // GET: Rental/Cancel/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int? id)
        {
            if ((id ?? 0) <= 0)
            {
                return NotFound();
            }
            var rentalRecord = await _context.Records.FirstOrDefaultAsync(m => m.RecordID == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }
            return View(rentalRecord);
        }

        // POST: Rental/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirm(int id)
        {
            var rentalRecord = await _context.Records.FindAsync(id);
            if (rentalRecord.Status == Status.Scheduled)
            {
                foreach (ItemRecord itemRecord in rentalRecord.ItemRecords)
                    _context.ItemRecords.Remove(itemRecord);
                _context.Records.Remove(rentalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        // GET: Rental/Delete/5
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rentalRecord = await _context.Records.FirstOrDefaultAsync(m => m.RecordID == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }
            return View(rentalRecord);
        }

        // POST: Rental/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalRecord = await _context.Records.FindAsync(id);
            foreach (ItemRecord itemRecord in rentalRecord.ItemRecords)
                _context.ItemRecords.Remove(itemRecord);
            _context.Records.Remove(rentalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool RentalRecordExists(int id)
        {
            return _context.Records.Any(e => e.RecordID == id);
        } 

        public async Task<PartialViewResult> AddRentalItemRecord(int ItemID, DateTime Start, DateTime End)
        {
            var Item = await _context.Items.FindAsync(ItemID);
            var isHoliday = await _context.Holidays.AnyAsync(day => day.Date.DayOfYear == Start.DayOfYear);
            List<Pricing> actualprices = await PricingFilter.ActualPricing(_context.Pricings, Item.ItemType.PricingCategoryID, Start, End, isHoliday);
            ViewBag.ActualPrices = actualprices;
            return PartialView("_ItemRecordRow_rental", new ItemRecord { ItemID = ItemID, Item = Item, Start = Start, End = End });
        }

        public async Task<PartialViewResult> AddPreparedItemRecord(int ItemID)
        {
            var Item = await _context.Items.FindAsync(ItemID);
            return PartialView("_ItemRow", Item);
        }

        public JsonResult Customer(int CustomerID)
        {
            return Json(_context.Customers.First(customer => customer.CustomerID == CustomerID));
        }

        public async Task<IActionResult> SearchCustomerByNumber(string Request)
        {
            if (Request == null)
            {
                return NotFound();
            }
            var trimmedRequest = ReplaceWhitespace(Request.Trim(), "");
            var foundCustomers = await _context.Customers.Where(customer => customer.CustomerPhoneNumber.Contains(trimmedRequest)).ToListAsync();
            return PartialView("_CustomersSearchResults", foundCustomers);
        }


    }
}
