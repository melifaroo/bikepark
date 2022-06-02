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
    public class PricingCategoriesController : Controller
    {
        private readonly BikeparkContext _context;

        public PricingCategoriesController(BikeparkContext context)
        {
            _context = context;
        }

        // GET: PricingCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.PricingCategories.ToListAsync());
        }

        // GET: PricingCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingCategory = await _context.PricingCategories
                .FirstOrDefaultAsync(m => m.PricingCategoryID == id);
            if (pricingCategory == null)
            {
                return NotFound();
            }

            return View(pricingCategory);
        }

        // GET: PricingCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PricingCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PricingCategoryID,PricingCategoryName")] PricingCategory pricingCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pricingCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pricingCategory);
        }

        // GET: PricingCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingCategory = await _context.PricingCategories.FindAsync(id);
            if (pricingCategory == null)
            {
                return NotFound();
            }
            return View(pricingCategory);
        }

        // POST: PricingCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PricingCategoryID,PricingCategoryName")] PricingCategory pricingCategory)
        {
            if (id != pricingCategory.PricingCategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricingCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricingCategoryExists(pricingCategory.PricingCategoryID))
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
            return View(pricingCategory);
        }

        // GET: PricingCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricingCategory = await _context.PricingCategories
                .FirstOrDefaultAsync(m => m.PricingCategoryID == id);
            if (pricingCategory == null)
            {
                return NotFound();
            }

            ViewData["ItemTypesCount"] = await _context.ItemTypes.Where(type => type.PricingCategoryID == id).CountAsync();
            ViewData["PricingsCount"] = await _context.Pricings.Where(pricing => pricing.PricingCategoryID == id).CountAsync();
            return View(pricingCategory);
        }

        // POST: PricingCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //ARCHIVE
            var pricingCategory = await _context.PricingCategories.FindAsync(id);
            if (pricingCategory != null)
            {
                LinqHelper.ForEach(_context.ItemTypes.Where(type => type.PricingCategoryID == id), type => type.PricingCategoryID = null);
                LinqHelper.ForEach(_context.Pricings.Where(pricing => pricing.PricingCategoryID == id), pricing => pricing.PricingCategoryID = null);
                _context.PricingCategories.Remove(pricingCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PricingCategoryExists(int id)
        {
          return _context.PricingCategories.Any(e => e.PricingCategoryID == id);
        }
    }
}
