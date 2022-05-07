#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;

namespace Bikepark.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly BikeparkContext _context;

        public ManageController(BikeparkContext context)
        {
            _context = context;
        }

        // GET: Storage
        public IActionResult Index()
        {
            return View();
        }

        // GET: Pricing
        public async Task<IActionResult> Pricing()
        {
            var pricing = _context.Set<RentalPricing>();//.Include(i => i.ItemType);
            return View(await pricing.ToListAsync());
        }

        // GET: Storage
        public async Task<IActionResult> Storage()
        {
            var storage = _context.Storage;//.Include(i => i.ItemType);
            return View(await storage.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> Storage([FromForm] ItemFilter filter)
        {
            var filtered = filter.FilterList(await _context.Storage.ToListAsync());
            return View("Storage", filtered);
        }


        // GET: Storage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Storage
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Storage/Create
        public IActionResult Create()
        {
            ViewData["ItemTypeID"] = new SelectList(_context.Set<ItemType>(), "ItemTypeID", "ItemName");
            return View();
        }

        // POST: Storage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ItemTypeID,ItemNumber,ItemStatus")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemTypeID"] = new SelectList(_context.Set<ItemType>(), "ItemTypeID", "ItemName", item.ItemTypeID);
            return View(item);
        }

        // GET: Storage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Storage.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemTypeID"] = new SelectList(_context.Set<ItemType>(), "ItemTypeID", "ItemName", item.ItemTypeID);
            return View(item);
        }

        // POST: Storage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,ItemTypeID,ItemNumber,ItemStatus")] Item item)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists((int)item.ItemID))
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
            ViewData["ItemTypeID"] = new SelectList(_context.Set<ItemType>(), "ItemTypeID", "ItemName", item.ItemTypeID);
            return View(item);
        }

        // GET: Storage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Storage
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Storage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Storage.FindAsync(id);
            _context.Storage.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Storage.Any(e => e.ItemID == id);
        }
    }
}
