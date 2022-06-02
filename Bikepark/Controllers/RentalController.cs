#nullable disable
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


        // GET: Rental/stat
        public async Task<IActionResult> Stat()
        {
            return Json(new
            {
                Scheduled = await _context.Records.Where(record => record.Status == Status.Scheduled).CountAsync(),
                ScheduledWarning = await _context.Records.Where(record => record.Status == Status.Scheduled).Where(record => record.Start < DateTime.Now.AddMinutes(60) ).CountAsync(),
                ScheduledOverdue = await _context.Records.Where(record => record.Status == Status.Scheduled).Where(record => record.Start < DateTime.Now).CountAsync(),
                Active = await _context.Records.Where(record => record.Status == Status.Active).CountAsync(),
                ActiveWarning = await _context.Records.Where(record => record.Status == Status.Active).Where(record => record.End < DateTime.Now.AddMinutes(30)).CountAsync(),
                ActiveOverdue = await _context.Records.Where(record => record.Status == Status.Active).Where(record => record.End < DateTime.Now).CountAsync(),
                Service = await _context.ItemRecords.Where(record => record.Status >= Status.Service && record.Status < Status.Fixed).CountAsync(),
                ServiceWarning = await _context.ItemRecords.Where(record => record.Status >= Status.Service && record.Status<Status.Fixed).Where(record => record.End < DateTime.Now.AddDays(1)).CountAsync(),
                ServiceNeedAction = await _context.ItemRecords.Where(record => record.Status == Status.Service).CountAsync(),
            });
        }

        // GET: Rental
        public async Task<IActionResult> Index()
        {
            return View(await _context.Records.OrderByDescending(record => record.Start).ToListAsync());
        }
        // GET: Rental
        public async Task<IActionResult> ServiceIndex()
        {
            return View(await _context.ItemRecords.Where(record => record.Status>=Status.Service && record.Status<Status.Fixed).OrderBy(record => record.End).ToListAsync());
        }

        // GET: Rental/OfType/5
        public async Task<IActionResult> OfType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            return View("ItemRecordsLog", await _context.ItemRecords.Where(irecord => irecord.Item.ItemTypeID == id).ToListAsync());
        }
        // GET: Rental/OfItem/5
        public async Task<IActionResult> OfItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            return View("ItemRecordsLog", await _context.ItemRecords.Where(irecord => irecord.ItemID == id).ToListAsync());
        }

        // GET: Rental/WithPricing/5
        public async Task<IActionResult> WithPricing(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            return View("ItemRecordsLog", await _context.ItemRecords.Where(irecord => irecord.PricingID == id).ToListAsync());
        }

        // GET: Rental/Active
        public async Task<IActionResult> Active()
        {
            return View(await _context.Records.Where(r => r.Status == Status.Active).OrderBy(record => record.End).ToListAsync());
        }

        // GET: Rental/Scheduled
        public async Task<IActionResult> Scheduled()
        {
            return View(await _context.Records.Where(r => r.Status == Status.Scheduled).OrderBy(record => record.Start).ToListAsync());
        }

        // GET: Rental/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.Records
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.RecordID == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }

            return View(rentalRecord);
        }

        // GET: Rental/Create
        public async Task<IActionResult> Create()
        {
            return await Control(new Record());
        }
        // GET: Rental/Create
        public IActionResult GetBackNumber()
        {
            return View();
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

        // GET: Rental/ControlFixing/5
        public async Task<IActionResult> Service(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<ItemRecord> itemRecords = await _context.ItemRecords.Where(ir => ir.RecordID == id && ir.Status>Status.Closed).ToListAsync();
            if (itemRecords == null || itemRecords.Count==0)
            {
                return NotFound();
            }
            return Service(itemRecords);
        }

        private async Task<IActionResult> Control(Record rentalRecord)
        {
            var Types = await _context.ItemTypes.ToListAsync();//.Where(x => !x.Archival)
            ViewData["Items"] = _context.Items;//.Where(x => !x.Archival)
            ViewData["Prices"] = _context.Pricings;//.Where(x => !x.Archival)
            ViewData["ArchivalPrices"] = _context.Pricings.IgnoreQueryFilters().ToListAsync();//.Where(x => !x.Archival)
            ViewData["Rents"] = _context.Records;
            ViewData["Customers"] = _context.Customers;
            ViewData["RentedItems"] = _context.ItemRecords;
            ViewData["Types"] = Types;

            ViewData["Availability"] = GetAvailability(rentalRecord.RecordID);

            ViewData["WorkingHoursStart"] = _config.Value.WorkingHoursStart;
            ViewData["WorkingHoursEnd"] = _config.Value.WorkingHoursEnd;
            ViewData["MinServiceDelayBetweenRents"] = _config.Value.MinServiceDelayBetweenRents;
            foreach (var irec in rentalRecord.ItemRecords)
                if (irec.Item == null && irec.ItemID != null)
                    irec.Item = await _context.Items.IgnoreQueryFilters().FirstOrDefaultAsync(ir => ir.ItemID == irec.ItemID);
            return View("Control", rentalRecord);
        }
        private IActionResult Service(IEnumerable<ItemRecord> itemRecords)
        {
            //ARCHIVE
            ViewData["Prices"] = _context.Pricings;//.Where(x => !x.Archival);
            return View("Service", itemRecords);
        }

        private Dictionary<int, List<RecordInfo>> GetAvailability( int? RecordID ) {

            var availability = _context.ItemRecords.Where(renteditem =>
                    renteditem.RecordID != RecordID && 
                    ((renteditem.Status > Status.Draft && renteditem.Status < Status.Closed) ||
                    (renteditem.Status > Status.Service && renteditem.Status < Status.Fixed)))
                    .ToLookup(
                            renteditem => renteditem.ItemID ?? -1,
                            renteditem => new RecordInfo {
                                Start = renteditem.Start ?? renteditem.Record.Start ?? DateTime.MaxValue,
                                End = 
                                    ((renteditem.Status==Status.Active || renteditem.Status == Status.OnService 
                                    && (renteditem.End ?? renteditem.Record.End ?? DateTime.MinValue)<DateTime.Now)? 
                                    DateTime.Now: 
                                    (renteditem.End ?? renteditem.Record.End ?? DateTime.MinValue))
                                    .AddHours(_config.Value.MinServiceDelayBetweenRents) ,
                                Status = renteditem.Status,
                                //renteditem.RecordID ?? -1
                            }
                        ).ToDictionary(x => x.Key, x => x.ToList());

            return availability;
        }

        public bool Overlap(List<RecordInfo> periods, RecordInfo record2) {
            foreach ( var record1 in periods ){ 
                if (RecordInfo.Overlap(record1, record2))
                    return true;
            }
            return false;  
        }

        public bool CheckAvailability(Record rentalRecord, Status Action, DateTime ActionTime) {
            var availability = GetAvailability( rentalRecord.RecordID );
            var record = new RecordInfo {
                Start = (rentalRecord.Status < Status.Active ? rentalRecord.Start : ActionTime) ?? DateTime.Now,
                End = rentalRecord.End ?? DateTime.Now,
                Status = Status.Draft
            };
            foreach (ItemRecord itemRecord in rentalRecord.ItemRecords.Where(ir => ir.Status < Status.Active)) {
                if (itemRecord.ItemID!=null && availability.ContainsKey( itemRecord.ItemID??-1 )) {
                    if (Overlap(availability[itemRecord.ItemID ?? -1], record ))
                        return false;
                }
            }
            return true;            
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

                            }
                            if (closeditems == rentalRecord.ItemRecords.Count && rentalRecord.Status == Status.Active)
                            { // close record
                                rentalRecord.Status = Status.Closed;
                                rentalRecord.End = CloseTime;
                            }



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
                        RedirectToAction(nameof(Service), new { id = rentalRecord.RecordID }) :
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ControlForNumber(NumberGetBack number) {

            if (ModelState.IsValid)
            {
                var itemRec = await _context.ItemRecords.FirstOrDefaultAsync(irec => irec.Item.ItemNumber == number.ItemNumber && irec.Status == Status.Active);
                if (itemRec == null || itemRec.Record==null) {
                    ViewData["error"] = "запись не найдена";
                    return View("GetBackNumber", number);
                }
                
                return RedirectToAction(nameof(Control), new { id = itemRec.Record.RecordID });
            }
            return View("GetBackNumber", number);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Record rentalRecord)
        {
            if (ModelState.IsValid && rentalRecord.Status == Status.Scheduled)
            {
                if ((rentalRecord.RecordID ?? 0) <= 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                return View(rentalRecord);
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

        // POST: Rental/CancelConfirm/5
        [HttpPost]
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

        // GET: Rental/ItemDetails/5
        public async Task<IActionResult> ItemDetails(int? id)
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
                .Where(r => r.Status == Status.Active || r.Status == Status.Scheduled)
                .Where(r => r.ItemID == id)
                .Include(r => r.Record)
                .OrderBy(r => r.Status)
                .ToListAsync();
            return View("ItemDetails", item);
        }

        public PartialViewResult AddRentedItem(int ItemID)
        {
            return PartialView("_ItemRecordRow", new ItemRecord { ItemID = ItemID, Item = _context.Items.FirstOrDefault(item => item.ItemID == ItemID) });
        }

        public JsonResult Customer(int CustomerID)
        {
            //ARCHIVE
            return Json(_context.Customers.First(customer => customer.CustomerID == CustomerID));
        }

        public async Task<IActionResult> SearchCustomerByNumber(string Request)
        {
            //ARCHIVE
            if (Request == null)
            {
                return NotFound();
            }
            var trimmedRequest = ReplaceWhitespace(Request.Trim(), "");
            var foundCustomers = await _context.Customers.Where(customer => customer.CustomerContactNumber.Contains(trimmedRequest)).ToListAsync();
            return PartialView("_CustomersSearchResults", foundCustomers);
        }
    }
}
