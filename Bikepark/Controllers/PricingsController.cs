﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bikepark.Data;
using Bikepark.Models;

namespace Bikepark.Controllers
{
    [Authorize(Roles = "BikeparkManagers")]
    public class PricingsController : Controller
    {
        private readonly BikeparkContext _context;

        public PricingsController(BikeparkContext context)
        {
            _context = context;
        }

        // GET: Pricings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pricings.ToListAsync());//.Where(x => !x.Archival)
        }

        // GET: Pricings/InCategory/5
        public async Task<IActionResult> InCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View("Index", await _context.Pricings.Where(type => type.PricingCategoryID == id).ToListAsync());//.Where(x => !x.Archival)
        }

        // GET: Pricings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.Pricings
                .FirstOrDefaultAsync(m => m.PricingID == id);
            if (pricing == null)
            {
                return NotFound();
            }

            return View(pricing);
        }

        // GET: Pricings/Create
        public async Task<IActionResult> Create()
        {
            return await EditForm(new Pricing());
        }

        // GET: Pricings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.Pricings.FindAsync(id);
            if (pricing == null)
            {
                return NotFound();
            }

            return await EditForm(pricing);
        }

        private async Task<IActionResult> EditForm(Pricing pricing)
        {
            ViewData["PricingCategoryID"] = new SelectList(_context.PricingCategories, "PricingCategoryID", "PricingCategoryName", pricing.PricingCategoryID);
            ViewData["HasRecords"] = pricing.PricingID == null ? false : await _context.ItemRecords.AnyAsync(irecord => irecord.Item.ItemTypeID == pricing.PricingID);
            return View("Edit", pricing);
        }

        // POST: Pricings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PricingID,PricingName,PricingCategoryID,PricingType,DaysOfWeek,IsHoliday,IsReduced,MinDuration,Price,ExtraPrice")] Pricing pricing)
        {
            if (ModelState.IsValid)
            {
                pricing.PricingID = null;
                _context.Add(pricing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = pricing.PricingID });
            }
            return await EditForm(pricing);
        }

        // POST: Pricings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( [Bind("PricingID,PricingName,PricingCategoryID,PricingType,DaysOfWeek,IsHoliday,IsReduced,MinDuration,Price,ExtraPrice")] Pricing pricing)
        {
            var id = pricing.PricingID;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pricing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PricingExists(pricing.PricingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = pricing.PricingID });
            }
            return await EditForm(pricing);
        }

        // POST: Pricings/Replace/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Replace([Bind("PricingID,PricingName,PricingCategoryID,PricingType,DaysOfWeek,IsHoliday,IsReduced,MinDuration,Price")] Pricing pricing)
        {
            var id = pricing.PricingID;

            if (ModelState.IsValid)
            {
                Pricing replace = new Pricing { 
                    PricingName = pricing.PricingName, 
                    PricingCategoryID = pricing.PricingCategoryID, 
                    PricingType = pricing.PricingType, 
                    DaysOfWeek = pricing.DaysOfWeek,
                    IsHoliday = pricing.IsHoliday,
                    IsReduced = pricing.IsReduced,
                    MinDuration = pricing.MinDuration,
                    Price = pricing.Price                    
                };
                _context.Add(replace);
                var HasRecords = await _context.ItemRecords.AnyAsync(irecord => irecord.PricingID == id);
                if (HasRecords)
                {
                    pricing.Archival = true;
                }
                else
                {
                    _context.Pricings.Remove(pricing);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = replace.PricingID });
            }
            return await EditForm(pricing);
        }

        // GET: Pricings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pricing = await _context.Pricings
                .FirstOrDefaultAsync(m => m.PricingID == id);
            if (pricing == null)
            {
                return NotFound();
            }

            ViewData["HasRecords"] = await _context.ItemRecords.AnyAsync(irecord => irecord.PricingID == id);
            return View(pricing);
        }

        // POST: Pricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            //ARCHIVE
            var pricing = await _context.Pricings.FindAsync(id);
            if (pricing != null)
            {
                var HasRecords = await _context.ItemRecords.AnyAsync(irecord => irecord.PricingID == id);
                if (HasRecords)
                {
                    pricing.Archival = true;
                }
                else
                {
                    _context.Pricings.Remove(pricing);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PricingExists(int? id)
        {
          return _context.Pricings.Any(e => e.PricingID == id);
        }
    }
}