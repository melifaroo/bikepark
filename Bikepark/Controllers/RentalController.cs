﻿#nullable disable
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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;

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
                ScheduledWarning = await _context.Records.Where(record => record.Status == Status.Scheduled).Where(record => record.Start < DateTime.Now.AddMinutes(60) ).CountAsync(),
                ScheduledOverdue = await _context.Records.Where(record => record.Status == Status.Scheduled).Where(record => record.Start < DateTime.Now).CountAsync(),
                Active = await _context.Records.Where(record => record.Status == Status.Active).CountAsync(),
                ActiveWarning = await _context.Records.Where(record => record.Status == Status.Active).Where(record => record.End < DateTime.Now.AddMinutes(30)).CountAsync(),
                ActiveOverdue = await _context.Records.Where(record => record.Status == Status.Active).Where(record => record.End < DateTime.Now).CountAsync(),
                Service = await _context.ItemRecords.Where(record => record.Status >= Status.Service && record.Status < Status.Fixed).CountAsync(),
                ServiceWarning = await _context.ItemRecords.Where(record => record.Status == Status.OnService).Where(record => record.End < DateTime.Now.AddDays(1)).CountAsync(),
                ServiceNeedAction = await _context.ItemRecords.Where(record => record.Status == Status.Service || (record.Status == Status.OnService && record.End < DateTime.Now )).CountAsync(),
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
            return View("ItemRecordsIndex_service", await _context.ItemRecords.Where(record => record.Status>=Status.Service && record.Status<Status.Fixed).OrderByDescending(record => record.End).ToListAsync());
        }

        // GET: Rental
        public async Task<IActionResult> ServiceIndexAll()
        {
            return View("ItemRecordsIndex_service_all", await _context.ItemRecords.Where(record => record.Status >= Status.Service && record.Status <= Status.Fixed).OrderByDescending(record => record.End).ToListAsync());
        }
        // GET: Rental
        public async Task<IActionResult> ServiceIndexActual()
        {
            return View("ItemRecordsIndex_service", await _context.ItemRecords.Where(record => record.Status >= Status.Service && record.Status < Status.Fixed).OrderByDescending(record => record.End).ToListAsync());
        }

        public async Task<IActionResult> RentalIndexAll()
        {
            return View("ItemRecordsIndex_rental_all", await _context.ItemRecords.Where(record => record.Status >= Status.Draft && record.Status < Status.Fixed).OrderByDescending(record => record.End).ToListAsync());
        }
        public async Task<IActionResult> RentalIndexActual()
        {
            return View("ItemRecordsIndex_rental", await _context.ItemRecords.Where(record => record.Status >= Status.Scheduled && record.Status < Status.Closed).OrderByDescending(record => record.End).ToListAsync());
        }

        public async Task<IActionResult> ItemRecordsIndex() { 
            return View("ItemRecordsIndex", await _context.ItemRecords.OrderByDescending(record => record.End).ToListAsync());        
        }

        // GET: Rental/OfType/5
        public async Task<IActionResult> OfType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            return View("ItemRecordsIndex", await _context.ItemRecords.Where(irecord => irecord.Item.ItemTypeID == id).OrderByDescending(record => record.End).ToListAsync());
        }
        // GET: Rental/OfItem/5
        public async Task<IActionResult> OfItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            return View("ItemRecordsIndex", await _context.ItemRecords.Where(irecord => irecord.ItemID == id).OrderByDescending(record => record.End).ToListAsync());
        }

        // GET: Rental/WithPricing/5
        public async Task<IActionResult> WithPricing(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //ARCHIVE
            return View("ItemRecordsIndex", await _context.ItemRecords.Where(irecord => irecord.PricingID == id).OrderByDescending(record => record.End).ToListAsync());
        }

        // GET: Rental/Active
        public async Task<IActionResult> Active()
        {
            return View("Index_active", await _context.Records.Where(r => r.Status == Status.Active).OrderBy(record => record.End).ToListAsync());
        }

        // GET: Rental/Scheduled
        public async Task<IActionResult> Scheduled()
        {
            return View("Index_scheduled", await _context.Records.Where(r => r.Status == Status.Scheduled).OrderBy(record => record.Start).ToListAsync());
        }

        // GET: Rental/Create
        public async Task<IActionResult> Create()
        {
            return await Control(new Record());
        }

        // GET: Rental/GetBackNumber
        public IActionResult GetBackNumber()
        {
            return View();
        }
        // GET: Rental/StartNewService
        public IActionResult StartNewService()
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

        private async Task<IActionResult> Control(Record rentalRecord)
        {
            ViewData["Items"] = _context.Items;
            ViewData["Types"] = await _context.ItemTypes.ToListAsync();
            ViewData["Pricings"] = _context.Pricings;
            ViewData["Holidays"] = await _context.Holidays.Select(day => day.Date).ToListAsync();
            ViewData["Prepared"] = await _context.Prepared.Select(prep=> prep.ItemID).ToListAsync();

            ViewData["ArchivalPricings"] = await _context.Pricings.IgnoreQueryFilters().ToListAsync();
            ViewData["Availability"] = GetAvailability(rentalRecord.RecordID);

            ViewData["WorkingHoursStart"] = _config.Value.WorkingHoursStart;
            ViewData["WorkingHoursEnd"] = _config.Value.WorkingHoursEnd;
            ViewData["MinServiceDelayBetweenRentsMinutes"] = _config.Value.MinServiceDelayBetweenRentsMinutes;
            ViewData["DefaultRentTimeHours"] = _config.Value.DefaultRentTimeHours;

            foreach (var irec in rentalRecord.ItemRecords)
                if (irec.Item == null && irec.ItemID != null)
                    irec.Item = await _context.Items.IgnoreQueryFilters().FirstOrDefaultAsync(ir => ir.ItemID == irec.ItemID);

            return View("Control", rentalRecord);
        }
        private Dictionary<int, List<ItemRecord>> GetAvailability( int? RecordID ) {

            var availability = _context.ItemRecords.Where(renteditem =>
                    renteditem.RecordID != RecordID && 
                    ((renteditem.Status > Status.Draft && renteditem.Status < Status.Closed) ||
                    (renteditem.Status > Status.Service && renteditem.Status < Status.Fixed)))
                    .ToLookup(
                            renteditem => renteditem.ItemID ?? -1,
                            renteditem => new ItemRecord {
                                Start = renteditem.Start ?? renteditem.Record.Start ?? DateTime.MaxValue,
                                End = 
                                    ((renteditem.Status==Status.Active || renteditem.Status == Status.OnService 
                                    && (renteditem.End ?? renteditem.Record.End ?? DateTime.MinValue)<DateTime.Now)? 
                                    DateTime.Now: 
                                    (renteditem.End ?? renteditem.Record.End ?? DateTime.MinValue))
                                    .AddMinutes(_config.Value.MinServiceDelayBetweenRentsMinutes) ,
                                Status = renteditem.Status,
                                ItemRecordID = renteditem.ItemRecordID
                            }
                        ).ToDictionary(x => x.Key, x => x.ToList());

            return availability;
        }

        public bool Overlap(List<ItemRecord> periods, ItemRecord record2, int? itemRecordID) {
            foreach ( var record1 in periods ){ 
                if (RecordInfo.Overlap(record1, record2) && record1.ItemRecordID!=itemRecordID)
                    return true;
            }
            return false;  
        }

        public bool CheckAvailability(Record rentalRecord, Status Action, DateTime ActionTime) {
            var availability = GetAvailability( rentalRecord.RecordID );
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
                        RedirectToAction(nameof(ControlService), new { id = rentalRecord.RecordID }) :
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

        public async Task<IActionResult> ExportAllRental() {
            var fileDownloadName = String.Format("FileName.xlsx");
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            // Pass your ef data to method
            ExcelPackage package = GenerateExcelFile(await _context.ItemRecords.ToListAsync());

            return RedirectToAction(nameof(RentalIndexAll));

        }

        public void ExportToExcel( IEnumerable<ItemRecord> datasource, string filename )
        {
            try
            {
                if (tbl == null || tbl.Columns.Count == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook
                var excelApp = new Excel.Application();
                var workbook = excelApp.Workbooks.Add();

                // single worksheet
                Excel._Worksheet workSheet = (Excel._Worksheet)excelApp.ActiveSheet;

                // column headings
                for (var i = 0; i < tbl.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
                }

                // rows
                for (var i = 0; i < tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (var j = 0; j < tbl.Columns.Count; j++)
                    {
                        workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
                    }
                }

                try
                {
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "StokBilgisi";
                    saveFileDialog.DefaultExt = ".xlsx";
                    System.Windows.
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    excelApp.Quit();
                    Console.WriteLine("Excel file saved!");
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                    + ex.Message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        private static ExcelPackage GenerateExcelFile(IEnumerable<ItemRecord> datasource)
        {

            ExcelPackage pck = new ExcelPackage();

            //Create the worksheet 
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Log");

            // Sets Headers
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Категория";
            ws.Cells[1, 3].Value = "Модель";
            ws.Cells[1, 4].Value = "Номер";
            ws.Cells[1, 5].Value = "Время выдачи";
            ws.Cells[1, 6].Value = "Время возврата";
            ws.Cells[1, 7].Value = "Статус";
            ws.Cells[1, 8].Value = "Тариф";
            ws.Cells[1, 9].Value = "Стоимость по тарифу";
            ws.Cells[1, 10].Value = "Тарификация";
            ws.Cells[1, 11].Value = "Номер заказа";
            ws.Cells[1, 12].Value = "Клиент";
            ws.Cells[1, 13].Value = "Телефон клиента";

            // Inserts Data
            for (int i = 0; i < datasource.Count(); i++)
            {
                ws.Cells[i + 2, 1].Value = datasource.ElementAt(i).ItemRecordID;
                if (datasource.ElementAt(i).Item != null)
                {
                    if (datasource.ElementAt(i).Item.ItemType != null)
                    {
                        ws.Cells[i + 2, 2].Value = datasource.ElementAt(i).Item.ItemType.ItemCategory.ItemCategoryName;
                        ws.Cells[i + 2, 3].Value = datasource.ElementAt(i).Item.ItemType.ItemTypeName;
                    }
                    ws.Cells[i + 2, 4].Value = datasource.ElementAt(i).Item.ItemNumber;
                }
                ws.Cells[i + 2, 5].Value = datasource.ElementAt(i).Start;
                ws.Cells[i + 2, 6].Value = datasource.ElementAt(i).End;
                ws.Cells[i + 2, 7].Value = datasource.ElementAt(i).Status;
                if (datasource.ElementAt(i).Pricing != null)
                {
                    ws.Cells[i + 2, 8].Value = datasource.ElementAt(i).Pricing.PricingName;
                    ws.Cells[i + 2, 9].Value = datasource.ElementAt(i).Pricing.Price;
                    ws.Cells[i + 2, 10].Value = datasource.ElementAt(i).Pricing.PricingType;
                }
                if (datasource.ElementAt(i).Record != null)
                {
                    ws.Cells[i + 2, 11].Value = datasource.ElementAt(i).RecordID;
                    ws.Cells[i + 2, 12].Value = datasource.ElementAt(i).Record.Customer.CustomerFullName;
                    ws.Cells[i + 2, 13].Value = datasource.ElementAt(i).Record.Customer.CustomerContactNumber;
                }
            }

            // Format Header of Table
            using (ExcelRange rng = ws.Cells["A1:C1"])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                rng.Style.Fill.BackgroundColor.SetColor(Color.Gold); //Set color to DarkGray 
                rng.Style.Font.Color.SetColor(Color.Black);
            }
            return pck;
        }

        private async Task<IActionResult> ControlService(IEnumerable<ItemRecord> itemRecords)
        {
            ViewData["Pricings"] =  _context.Pricings;
            ViewData["ArchivalPricings"] = await _context.Pricings.IgnoreQueryFilters().ToListAsync();
            ViewData["Availability"] = GetAvailability(null);
            return View("ControlService", itemRecords);
        }

        private async Task<IActionResult> Service(ItemRecord itemRecord)
        {
            return await ControlService( new List<ItemRecord>() { itemRecord });
        }

        // GET: Rental/ControlService/5
        public async Task<IActionResult> ControlService(int? id)
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

        // GET: Rental/ControlItemService/5
        public async Task<IActionResult> ControlItemService(int? id)
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
            if (irec.Status < Status.Service)
            {
                return NotFound();
                //return RedirectToAction(nameof(ServiceIndex));
            }
            return await Service(irec);
        }

        // POST: Rental/SaveServiceRecords
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveServiceRecords(IEnumerable<Bikepark.Models.ItemRecord> itemRecords)
        {
            if (ModelState.IsValid)
            {
                //if (!CheckServiceAvailability(itemRecords))
                //{
                //    ViewData["Error"] = "Некоторые позиции забронированы на указанное время ремонта";
                //    return await ControlService(itemRecords);
                //}


                foreach (ItemRecord itemRecord in itemRecords) {
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
                return RedirectToAction(nameof(ServiceIndex));
            }
            else
            {
                return await ControlService(itemRecords);
            }
        }
        // GET: Rental/ControlItemService/5
        public async Task<IActionResult> ItemServiceFixed(int? id)
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
            return RedirectToAction(nameof(ServiceIndex));
        }
        

        // POST: Rental/SaveServiceRecords
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PrepareUpdate(IEnumerable<Item> items)
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
                    return View("StartNewService", number);
                }
                return await Service(new ItemRecord { ItemID = item.ItemID, Item = item, Start = DateTime.Now, End = DateTime.Now.AddDays(1), Status = Status.Service });
            }
            return View("StartNewService", number);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ControlForNumber(Number number) {

            if (ModelState.IsValid)
            {
                var itemRec = await _context.ItemRecords.FirstOrDefaultAsync(irec => irec.Item.ItemNumber == number.ItemNumber && irec.Status == Status.Active);
                if (itemRec == null || itemRec.Record==null) {
                    ViewData["Error"] = "запись не найдена";
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
        public async Task<IActionResult> DeleteItemRecord(int? id)
        {
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


        // POST: Rental/Delete/5
        [HttpPost, ActionName("DeleteItemRecord")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItemRecordConfirmed(int id)
        {
            var rentalRecord = await _context.ItemRecords.FindAsync(id);
            _context.ItemRecords.Remove(rentalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ItemRecordsIndex));
        }

        private bool RentalRecordExists(int id)
        {
            return _context.Records.Any(e => e.RecordID == id);
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
                .Where(r => r.Status != Status.Draft && r.Status != Status.Closed && r.Status != Status.Fixed )
                .Where(r => r.ItemID == id)
                .Include(r => r.Record)
                .OrderBy(r => r.Status)
                .ToListAsync();
            return View(item);
        }

        public async Task<PartialViewResult> AddRentalItemRecord(int ItemID, DateTime Start, DateTime End)
        {
            var Item = await _context.Items.FindAsync(ItemID);
            var isHoliday = await _context.Holidays.AnyAsync(day => day.Date.DayOfYear == Start.DayOfYear);
            List<Pricing> actualprices = await PricingFilter.ActualPricing(_context.Pricings, Item.ItemType.PricingCategoryID, Start, End, isHoliday);
            ViewBag.ActualPrices = actualprices;
            return PartialView("_ItemRecordRow_rental", new ItemRecord { ItemID = ItemID, Item = Item, Start = Start, End = End });
        }

        public async Task<PartialViewResult> AddServiceItemRecord(int ItemID, DateTime Start, DateTime End, int? RecordID)
        {
            var Item = await _context.Items.FindAsync(ItemID);
            ViewData["Pricings"] = _context.Pricings;
            return PartialView("_ItemRecordRow_service", new ItemRecord { ItemID = ItemID, Item = Item, Start = Start, End = End, Status = Status.Service, RecordID = RecordID });
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
            var foundCustomers = await _context.Customers.Where(customer => customer.CustomerContactNumber.Contains(trimmedRequest)).ToListAsync();
            return PartialView("_CustomersSearchResults", foundCustomers);
        }


    }
}
