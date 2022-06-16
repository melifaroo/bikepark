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

namespace Bikepark.Controllers
{
    public class LogController : Controller
    {
        private readonly BikeparkContext _context;
        private readonly Status[] AllStatuses = Enum.GetValues(typeof(Status)).Cast<Status>().ToArray();

        public LogController(BikeparkContext context)
        {
            _context = context;
        }

        private IActionResult Log(IEnumerable<ItemRecord> log, string logName, Status[]? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            ViewData["LogName"] = logName;
            ViewData["Statuses"] = statuses != null?statuses.Cast<int>():null;
            ViewData["From"] = from;
            ViewData["To"] = to;
            return View("Index", log);
        }

        // GET: Log
        public async Task<IActionResult> Index(string? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            statuses = (statuses == null || statuses.Length==0) ? (string.Join(",", AllStatuses.Cast<int>())) : statuses;
            var Statuses = statuses.Split(",").Select(Int32.Parse).Cast<Status>().ToArray();
            var name = "Записи" ;
            var log = await _context.ItemRecords.Where(irecord => Statuses.Contains(irecord.Status) ).Where(irecord => (to == null || irecord.Start <= ((DateTime)to).AddDays(1)) && (from == null || irecord.Start >= ((DateTime)from))).OrderByDescending(irecord => irecord.End).ToListAsync();//.Include(i => i.Item).Include(i => i.Pricing).Include(i => i.Record).Include(i => i.User);
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
            var log = await _context.ItemRecords.Where(irecord => irecord.Item.ItemTypeID == id).OrderByDescending(irecord => irecord.End).ToListAsync();
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
            var log = await _context.ItemRecords.Where(irecord => irecord.ItemID == id).OrderByDescending(record => record.End).ToListAsync();
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
            var log = await _context.ItemRecords.Where(irecord => irecord.PricingID == id).OrderByDescending(record => record.End).ToListAsync();
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
                return RedirectToAction(nameof(Index), new { statuses = new Status[] { Status.Service, Status.OnService  } });
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
            return RedirectToAction(nameof(Index), new { statuses = new Status[] { Status.Service, Status.OnService } });
        }

        public async Task<FileResult> Export(string? statuses = null, DateTime? from = null, DateTime? to = null)
        {
            var fileName = Guid.NewGuid().ToString() + ".csv";
            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

            //window.open(sitePath + "Controller/DownloadFile?fileid=" + id, '_blank');

            statuses = (statuses == null || statuses.Length == 0) ? (string.Join(",", AllStatuses.Cast<int>())) : statuses;
            var Statuses = statuses.Split(",").Select(Int32.Parse).Cast<Status>().ToArray();
            var log = await _context.ItemRecords.Where(irecord => Statuses.Contains(irecord.Status)).Where(irecord => (to == null || irecord.Start <= ((DateTime)to).AddDays(1)) && (from == null || irecord.Start >= ((DateTime)from))).OrderByDescending(irecord => irecord.End).ToListAsync();//.Include(i => i.Item).Include(i => i.Pricing).Include(i => i.Record).Include(i => i.User);

            ExportToFile(log, filePath);


            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);


        }

        public void ExportToFile(IEnumerable<ItemRecord> datasource, string filePath)
        {
            Console.WriteLine();
            try
            {
                var csv = new StringBuilder();
                var Cells = new string[datasource.Count() + 1,13];
                // column headings
                Cells[0, 0] = "ID";
                Cells[0, 1] = "Категория";
                Cells[0, 2] = "Модель";
                Cells[0, 3] = "Номер";
                Cells[0, 4] = "Время выдачи";
                Cells[0, 5] = "Время возврата";
                Cells[0, 6] = "Статус";
                Cells[0, 7] = "Тариф";
                Cells[0, 8] = "Стоимость по тарифу";
                Cells[0, 9] = "Тарификация";
                Cells[0, 10] = "Номер заказа";
                Cells[0, 11] = "Клиент";
                Cells[0, 12] = "Телефон клиента";

                // rows
                for (int i = 0; i < datasource.Count(); i++)
                {
                    Cells[i + 1, 0] = datasource.ElementAt(i).ItemRecordID?.ToString();
                    if (datasource.ElementAt(i).Item != null)
                    {
                        if (datasource.ElementAt(i).Item.ItemType != null)
                        {
                            if (datasource.ElementAt(i).Item.ItemType.ItemCategory != null)
                            {
                                Cells[i + 1, 1] = datasource.ElementAt(i).Item.ItemType.ItemCategory.ItemCategoryName;
                            }
                            Cells[i + 1, 2] = datasource.ElementAt(i).Item.ItemType.ItemTypeName;
                        }
                        Cells[i + 1, 3] = datasource.ElementAt(i).Item.ItemNumber;
                    }
                    Cells[i + 1, 4] = datasource.ElementAt(i).Start?.ToString();
                    Cells[i + 1, 5] = datasource.ElementAt(i).End?.ToString();
                    Cells[i + 1, 6] = datasource.ElementAt(i).Status.ToString();
                    if (datasource.ElementAt(i).Pricing != null)
                    {
                        Cells[i + 1, 7] = datasource.ElementAt(i).Pricing.PricingName;
                        Cells[i + 1, 8] = datasource.ElementAt(i).Pricing.Price.ToString();
                        Cells[i + 1, 9] = datasource.ElementAt(i).Pricing.PricingType.ToString();
                    }
                    if (datasource.ElementAt(i).Record != null)
                    {
                        Cells[i + 1, 10] = datasource.ElementAt(i).RecordID?.ToString();
                        Cells[i + 1, 11] = datasource.ElementAt(i).Record.Customer.CustomerFullName;
                        Cells[i + 1, 12] = datasource.ElementAt(i).Record.Customer.CustomerContactNumber;
                    }
                }

                using (StreamWriter writer = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write)))
                {
                    for (int i = 0; i < datasource.Count() + 1; i++) {
                        for (int j = 0; j < 13; j++)
                        {
                            writer.Write(Cells[i, j] + "\t");
                        }
                        writer.Write("\n");
                    }
                }
                

            }
            catch (Exception ex)
            {
                throw new Exception("ExportToFile: \n" + ex.Message);
            }
        }

        //public async Task<FileResult> ExportAllRental()
        //{
        //    const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    var fileName = Path.Combine("/Temp/", Guid.NewGuid().ToString(), ".xls");
        //    //window.open(sitePath + "Controller/DownloadFile?fileid=" + id, '_blank');
        //    ExportToExcel(await _context.ItemRecords.ToListAsync(), fileName);
        //    return File(fileName, contentType, "");
        //}

        //public void ExportToExcel(IEnumerable<ItemRecord> datasource, string filename)
        //{
        //    try
        //    {

        //        // load excel, and create a new workbook
        //        var excelApp = new Excel.Application();
        //        var workbook = excelApp.Workbooks.Add();

        //        // single worksheet
        //        Excel._Worksheet ws = (Excel._Worksheet)excelApp.ActiveSheet;

        //        // column headings
        //        ws.Cells[1, 1] = "ID";
        //        ws.Cells[1, 2] = "Категория";
        //        ws.Cells[1, 3] = "Модель";
        //        ws.Cells[1, 4] = "Номер";
        //        ws.Cells[1, 5] = "Время выдачи";
        //        ws.Cells[1, 6] = "Время возврата";
        //        ws.Cells[1, 7] = "Статус";
        //        ws.Cells[1, 8] = "Тариф";
        //        ws.Cells[1, 9] = "Стоимость по тарифу";
        //        ws.Cells[1, 10] = "Тарификация";
        //        ws.Cells[1, 11] = "Номер заказа";
        //        ws.Cells[1, 12] = "Клиент";
        //        ws.Cells[1, 13] = "Телефон клиента";

        //        // rows
        //        for (int i = 0; i < datasource.Count(); i++)
        //        {
        //            ws.Cells[i + 2, 1] = datasource.ElementAt(i).ItemRecordID;
        //            if (datasource.ElementAt(i).Item != null)
        //            {
        //                if (datasource.ElementAt(i).Item.ItemType != null)
        //                {
        //                    ws.Cells[i + 2, 2] = datasource.ElementAt(i).Item.ItemType.ItemCategory.ItemCategoryName;
        //                    ws.Cells[i + 2, 3] = datasource.ElementAt(i).Item.ItemType.ItemTypeName;
        //                }
        //                ws.Cells[i + 2, 4] = datasource.ElementAt(i).Item.ItemNumber;
        //            }
        //            ws.Cells[i + 2, 5] = datasource.ElementAt(i).Start;
        //            ws.Cells[i + 2, 6] = datasource.ElementAt(i).End;
        //            ws.Cells[i + 2, 7] = datasource.ElementAt(i).Status;
        //            if (datasource.ElementAt(i).Pricing != null)
        //            {
        //                ws.Cells[i + 2, 8] = datasource.ElementAt(i).Pricing.PricingName;
        //                ws.Cells[i + 2, 9] = datasource.ElementAt(i).Pricing.Price;
        //                ws.Cells[i + 2, 10] = datasource.ElementAt(i).Pricing.PricingType;
        //            }
        //            if (datasource.ElementAt(i).Record != null)
        //            {
        //                ws.Cells[i + 2, 11] = datasource.ElementAt(i).RecordID;
        //                ws.Cells[i + 2, 12] = datasource.ElementAt(i).Record.Customer.CustomerFullName;
        //                ws.Cells[i + 2, 13] = datasource.ElementAt(i).Record.Customer.CustomerContactNumber;
        //            }
        //        }

        //        try
        //        {
        //            workbook.SaveAs(filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //            excelApp.Quit();
        //            Console.WriteLine("Excel file saved!");
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n" + ex.Message);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("ExportToExcel: \n" + ex.Message);
        //    }
        //}

    }
}
