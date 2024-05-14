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
- This controller manages the CRUD operations for products in the PcPulse application.
- It allows adding, editing, and deleting products, as well as viewing the list of products.
- The controller interacts with the PcPulse database context to perform database operations.
*/


using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PcPulse.Areas.Identity.Data;
using PcPulse.Models;

namespace PcPulse.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PcPulseDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly INotyfService _Notification;
        public ProductsController(PcPulseDbContext context, IWebHostEnvironment hostEnvironment, INotyfService notyf)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _Notification = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "ShortName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                string dateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string webRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ProductFile.FileName);
                string extension = Path.GetExtension(product.ProductFile.FileName);
                string uniqueFileName = fileName + "_" + dateTime + extension;

                product.ProductPicture = uniqueFileName;
                string filePath = Path.Combine(webRootPath, "ProductImage", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.ProductFile.CopyTo(fileStream);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                _Notification.Success("Product is added Successfully.");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "ShortName", product.CategoryId);
            _Notification.Error("Error! Adding Product, Please try again.");
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "ShortName", product.CategoryId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ProductFile != null)
                    {
                        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        string webRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(product.ProductFile.FileName);
                        string extension = Path.GetExtension(product.ProductFile.FileName);
                        string uniqueFileName = fileName + "_" + dateTime + extension;

                        product.ProductPicture = uniqueFileName;
                        string filePath = Path.Combine(webRootPath, "ProductImage", uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            product.ProductFile.CopyTo(fileStream);
                        }


                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _Notification.Success("Product is updated Successfully");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "ShortName", product.CategoryId);
            _Notification.Error("Error! Updating Product. Please try again");
            return View(product);
        }
        public IActionResult Delete(int id)
        {
            var getData = _context.Products.FirstOrDefault(x => x.Id == id);
            if (getData != null)
            {
                _context.Products.Remove(getData);
                _context.SaveChanges();
                _Notification.Success("Product is deleted successfully.");
                return Json(new { success = true, message = "Product is deleted successfully" });
            }
            else
            {
                _Notification.Error("Error! Deleting Product. Please try again.");
                return Json(new { success = false, message = "Error! Deleting Product. Please try again." });
            }
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
