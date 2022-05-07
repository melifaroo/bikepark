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

namespace Bikepark.Controllers
{
    public class RentalController : Controller
    {
        private readonly BikeparkContext _context;

        public RentalController(BikeparkContext context)
        {
            _context = context;
        }

        // GET: Rental
        public async Task<IActionResult> Index()
        {
            var bikeparkContext = _context.RentalLog;//.Include(r => r.Customer);
            return View(await bikeparkContext.ToListAsync());
        }

        // GET: Rental/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalLog
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.RentalRecordID == id);
            if (rentalRecord == null)
            {
                return NotFound();
            }

            return View(rentalRecord);
        }

        // GET: Rental/Create
        public IActionResult Create()
        {
            ViewData["Customers"] = _context.Set<Customer>();
            ViewData["Items"] = _context.Storage;
            ViewData["Prices"] = _context.Set<RentalPricing>();
            ViewData["Types"] = _context.Set<ItemType>();
            ViewData["Rents"] = _context.RentalLog;
            ViewData["RentedItems"] = _context.Set<RentalItem>();
            return View("RentalControl", new RentalRecord());
        }

        public PartialViewResult AddRentedItem(int? ItemID)
        {
            ViewData["Prices"] = _context.Set<RentalPricing>();
            return PartialView("_RentalRecordItemPartial", new RentalItem { ItemID = ItemID, Item = _context.Storage.FirstOrDefault(item => item.ItemID == ItemID) });
        }

        // GET: Rental/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalLog.FindAsync(id);
            if (rentalRecord == null)
            {
                return NotFound();
            }
            ViewData["Customers"] = _context.Set<Customer>();
            ViewData["Items"] = _context.Storage;
            ViewData["Prices"] = _context.Set<RentalPricing>();
            ViewData["Types"] = _context.Set<ItemType>();
            ViewData["Rents"] = _context.RentalLog;
            ViewData["RentedItems"] = _context.Set<RentalItem>();
            return View("RentalControl", rentalRecord);
        }


        // POST: Rental/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalRecord rentalRecord)//[FromForm][Bind("RentalRecordID,CustomerID,RentalStatus,RentalItems,RentalTermInHours,RentalReservedStart,RentalStart,RentalEnd")] 
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentalRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customers"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerFullName", rentalRecord.CustomerID);
            return View("RentalControl", rentalRecord);
        }

        //// POST: Rental/CreateCustomer
        //public async Task<IActionResult> CreateCustomer(RentalRecord rentalRecord)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Customer customer;
        //        _context.Add(customer = new Customer
        //        {
        //            CustomerFullName = rentalRecord.Customer.CustomerFullName,
        //            CustomerEMail = rentalRecord.Customer.CustomerEMail,
        //            CustomerPassport = rentalRecord.Customer.CustomerPassport,
        //            CustomerContactNumber = rentalRecord.Customer.CustomerContactNumber,
        //        } );
        //        await _context.SaveChangesAsync();

        //        rentalRecord.CustomerID = customer.CustomerID;
        //        ViewData["Customers"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerFullName", rentalRecord.CustomerID);
        //        return RedirectToAction(nameof(Create), rentalRecord);
        //    }
        //    return RedirectToAction(nameof(Create), rentalRecord);
        //}

        // POST: Rental/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalRecordID,CustomerID,RentalStatus,RentalTermInHours,RentalReservedStart,RentalStart,RentalEnd")] RentalRecord rentalRecord)
        {
            if (id != rentalRecord.RentalRecordID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalRecordExists((int)rentalRecord.RentalRecordID))
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
            ViewData["CustomerID"] = new SelectList(_context.Set<Customer>(), "CustomerID", "CustomerID", rentalRecord.CustomerID);
            return View("RentalControl", rentalRecord);
        }



        // GET: Rental/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalRecord = await _context.RentalLog
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(m => m.RentalRecordID == id);
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
            var rentalRecord = await _context.RentalLog.FindAsync(id);
            _context.RentalLog.Remove(rentalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalRecordExists(int id)
        {
            return _context.RentalLog.Any(e => e.RentalRecordID == id);
        }
    }
}
