using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;

namespace Bikepark.Controllers
{
    [Authorize(Roles = "BikeparkManagers")]
    public class ItemsController : Controller
    {
        private readonly BikeparkContext _context;

        public ItemsController(BikeparkContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _context.Items.ToListAsync());//.Where(x => !x.Archival)
        }

        // GET: Items/OfType
        public async Task<IActionResult> OfType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View("Index", await _context.Items.Where(item => item.ItemTypeID == id).ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public async Task<IActionResult> Create()
        {
            return await EditForm(new Item()); 
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return await EditForm(item);
        }

        private async Task<IActionResult> EditForm(Item item)
        {
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ItemTypeID", "ItemTypeName", item.ItemTypeID);
            ViewData["HasRecords"] = item.ItemID==null?false:await _context.ItemRecords.AnyAsync(irecord => irecord.ItemID == item.ItemID);
            return View("Edit", item);
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,ItemTypeID,ItemNumber")] Item item)
        {
            if (ModelState.IsValid)
            {
                item.ItemID = null;
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = item.ItemID });
            }
            return await EditForm(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ItemID,ItemTypeID,ItemNumber")] Item item)
        {
            var id = item.ItemID;

            if (ModelState.IsValid)
            {
                if (await _context.Items.AsNoTracking().AnyAsync(itemDB => itemDB.ItemNumber == item.ItemNumber))
                {
                    ViewData["Error"] = "номер используется " + item.ItemNumber;
                    return await EditForm(item);
                }
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = item.ItemID });
            }
            return await EditForm(item);
        }

        // POST: Items/Replace/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Replace([Bind("ItemID,ItemTypeID,ItemNumber")] Item item)
        {
            var id = item.ItemID;

            if (ModelState.IsValid)
            {
                if (await _context.Items.AsNoTracking().AnyAsync(itemDB => itemDB.ItemNumber == item.ItemNumber))
                {
                    ViewData["Error"] = "номер используется " + item.ItemNumber;
                    return await EditForm(item);
                }

                Item replace = new Item { 
                    ItemNumber = item.ItemNumber, 
                    ItemTypeID = item.ItemTypeID 
                };
                _context.Add(replace);
                var HasRecords = await _context.ItemRecords.AnyAsync(irecord => irecord.ItemID == id);
                if (HasRecords)
                {
                    item.Archival = true;
                }
                else
                {
                    _context.Items.Remove(item);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = replace.ItemID });
            }
            return await EditForm(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemType)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["HasRecords"] = await _context.ItemRecords.AnyAsync(irecord => irecord.ItemID == id);
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            //ARCHIVE
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                var HasRecords = await _context.ItemRecords.AnyAsync(irecord => irecord.ItemID == id);
                if (HasRecords)
                {
                    item.Archival = true;
                }
                else
                {
                    _context.Items.Remove(item);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int? id)
        {
          return _context.Items.Any(e => e.ItemID == id);
        }
    }
}
