/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- This controller handles CRUD operations for categories in the PcPulse application.
- It uses PcPulseDbContext for database operations and INotyfService for toast notifications.
*/

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PcPulse.Areas.Identity.Data;
using PcPulse.Models;

namespace PcPulse.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly PcPulseDbContext _context;
        private readonly INotyfService _Notification;
        public CategoriesController(PcPulseDbContext context, INotyfService notyf)
        {
            _context = context;
            _Notification = notyf;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Sorry, Entity set 'PcPulseDbContext.Categories'  is null.");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShortName,LongName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                _Notification.Success("Category is Created successfully");
                return RedirectToAction(nameof(Index));
            }
            _Notification.Error("Error! Creating Category. Please Try Again");
            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShortName,LongName")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _Notification.Success("Category is Updated successfully");
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            _Notification.Error("Error! Updating Category. Please try Again.");
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var getData = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (getData != null)
            {
                _context.Categories.Remove(getData);
                _context.SaveChanges();
                _Notification.Success("Category is deleted Successfully");
                return Json(new { success = true, message = "Category is deleted Successfully" });
            }
            else
            {
                _Notification.Error("Error! Deleting Category. Please try Again.");
                return Json(new { success = false, message = "Error! Deleting Category. Please try Again." });
            }
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
