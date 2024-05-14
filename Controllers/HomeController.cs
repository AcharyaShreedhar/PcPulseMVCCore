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
- This controller handles various actions related to the home page, product management, cart management, checkout process, order history, and user logout functionality.
- It interacts with the PcPulse database context to perform database operations.
*/

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PcPulse.Areas.Identity.Data;
using PcPulse.Models;
using PcPulse.SessionHelper;
using PcPulse.ViewModel;
using System.Diagnostics;

namespace PcPulse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PcPulseDbContext _context;
        private readonly INotyfService _Notification;
        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, PcPulseDbContext application, UserManager<ApplicationUser> userManager, INotyfService Notification)
        {
            _logger = logger;
            _signInManager = signInManager;
            _context = application;
            _userManager = userManager;
            _Notification = Notification;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Product(int? categoryId)
        {

            ViewBag.Category = new SelectList(_context.Categories, "Id", "ShortName");
            if (categoryId > 0)
            {
                var obj = _context.Products.Where(x => x.CategoryId == categoryId).ToList();
                return View(obj);
            }
            else
            {
                var obj = _context.Products.ToList();
                return View(obj);
            }
        }
        public IActionResult ProductDetails(int id)
        {
            var obj = _context.Products.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefault();
            return View(obj);
        }
        public IActionResult Cart()
        {
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");

            if (cart == null)
            {
                cart = new List<ProductItem>();
            }
            if (cart.Count() > 0)
            {
                var notificationMessage = TempData["NotificationMessage"];
                if (notificationMessage != null)
                {
                    _Notification.Success($"{notificationMessage}");
                }
            }

            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.Find(id);
            // Retrieving cart from session.
            List<ProductItem> cart = HttpContext.Session.Get<List<ProductItem>>("cart") ?? new List<ProductItem>();
            // Checking if product is already in cart.
            var existingCartItem = cart.FirstOrDefault(item => item.Product.Id == id);
            if (existingCartItem != null)
            {
                // If product already exists in cart, increasing the  quantity of the product
                existingCartItem.Quantity++;
            }
            else
            {
                // If product is not in cart, adding the product to the cart.
                cart.Add(new ProductItem { Product = product, Quantity = 1 });
            }
            // Updating product information in cart.
            foreach (var item in cart)
            {
                item.Product = _context.Products.Find(item.Product.Id);
            }
            // Updating session with updated cart
            HttpContext.Session.Set<List<ProductItem>>("cart", cart);
            // Notifying user
            _Notification.Success("Product is successfully added to the cart.");

            return RedirectToAction("Cart");

        }
        public IActionResult PlusQty(int id)
        {
            // Retrieving the available quantity of the product from the database
            var availableQuantity = _context.Products
                .Where(x => x.Id == id)
                .Select(x => x.ProductQuantity)
                .FirstOrDefault();
            // Retrieving the cart from the session
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            // Finding the index of the product in the cart
            int index = cart.FindIndex(w => w.Product.Id == id);
            if (index != -1)
            {
                // Checking If the product is already in the cart
                if (cart[index].Quantity < availableQuantity)
                {
                    //  Increasing the quantity,If the quantity in the cart is less than the available quantity.
                    TempData["NotificationMessage"] = "Quantity of the Product increased successfully.";
                    cart[index].Quantity++;
                }
                else
                {
                    // If the quantity in the cart is equal to or greater than the available quantity, redirecting to the cart
                    TempData["NotificationMessage"] = "Sorry,Maximum quantity reached for this product.";
                    return RedirectToAction("Cart");
                }
            }
            // Updating the session with the updated cart
            HttpContext.Session.Set<List<ProductItem>>("cart", cart);
            // Redirecting to the cart page
            return RedirectToAction("Cart");


        }
        public IActionResult MinusQty(int id)
        {
            // Retrieving the cart from the session
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");

            // Finding the index of the product in the cart
            var index = cart.FindIndex(m => m.Product.Id == id);

            // If the quantity of the product in the cart is 1, removing the item from the cart
            if (cart[index].Quantity == 1)
            {
                cart.RemoveAt(index);
                TempData["NotificationMessage"] = "Product is successfully removed from cart.";

            }
            else
            {
                // If the quantity of the product is greater than 1, decreasing the quantity by 1
                cart[index].Quantity--;
                TempData["NotificationMessage"] = " Quantity of the product is decreased successfully.";

            }

            // Updating  the session with the updated cart
            HttpContext.Session.Set<List<ProductItem>>("cart", cart);

            // Redirecting to the cart page
            return RedirectToAction("Cart");
        }

        public IActionResult Remove(int id)
        {
            // Checking  if the provided id is valid (greater than 0)
            if (id > 0)
            {
                // Retrieving the cart from the session
                var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
                // Finding  the index of the product in the cart
                var index = cart.FindIndex(m => m.Product.Id == id);
                // Removing the product from the cart
                cart.RemoveAt(index);
                // Updating the session with the updated cart
                HttpContext.Session.Set<List<ProductItem>>("cart", cart);
                TempData["NotificationMessage"] = "Product is successfully removed from cart.";
            }
            else
            {
                // clearing the cart in the session,If the provided id is not valid
                HttpContext.Session.Set<List<ProductItem>>("cart", null);
            }

            // Redirecting  to the cart page.
            return RedirectToAction("Cart");
        }


        public IActionResult CheckOut()
        {
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");
            int cartCount = cart?.Sum(s => s.Quantity) ?? 0;
            decimal total = cart?.Sum(s => s.Quantity * s.Product.ProductPrice) ?? 0;

            ViewBag.CartCount = cartCount != 0 ? cartCount : (int?)null;
            ViewBag.Total = total;
            return View();
        }
        [HttpPost]
        public IActionResult CheckOut(Order order)
        {
            int orderNumber = GenerateRandomNumber();
            var UserId = _context.Users.Where(x => x.Email == order.Email).Select(x => x.Id).FirstOrDefault();
            order.OrderNumber = orderNumber;
            order.OrderDate = DateTime.Now;
            order.OrderType = "Pending";
            order.userId = UserId;
            _context.Orders.Add(order);
            _context.SaveChanges();
            CreateOrderDetails(order);

            HttpContext.Session.Remove("cart");
            _Notification.Success("Congratulations, Your order has been confirmed successfully.");
            return RedirectToAction("OrderHistory");
        }
        public IActionResult OrderHistory()
        {
            string userid = _userManager.GetUserId(User);
            var obj = (from od in _context.OrderDetails
                       join order in _context.Orders on od.OrderId equals order.Id
                       join pro in _context.Products on od.productId equals pro.Id
                       where order.userId == userid && order.OrderType != "cancelled"
                       select new OrderViewModel
                       {
                           Address = order.Address,
                           FirstName = order.FirstName,
                           LastName = order.LastName,
                           MobileNumber = order.PhoneNumber,
                           orderid = order.Id,
                           productName = pro.ProductName,
                           productPicture = pro.ProductPicture,
                           productQuantity = od.Quantity,
                           OrderNumber = order.OrderNumber.ToString(),
                           OrderDate = order.OrderDate.Date.ToString(),
                           Total = (od.Quantity * od.SalePrice),
                           ProductPrice = od.SalePrice.ToString(),
                           OrderStatus = order.OrderType,
                       }).ToList();
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            _Notification.Success("Log Out Successfull.");
            return RedirectToAction("Index", "Home");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int GenerateRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000000, 999999999);
            return randomNumber;
        }
        private void CreateOrderDetails(Order order)
        {
            var cart = HttpContext.Session.Get<List<ProductItem>>("cart");

            foreach (var cartItem in cart)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = cartItem.Quantity,
                    SalePrice = cartItem.Product.ProductPrice,
                    OrderId = order.Id,
                    productId = cartItem.Product.Id
                };

                _context.OrderDetails.Add(orderDetail);
                _context.SaveChanges();

                UpdateProductQuantity(cartItem.Product.Id, cartItem.Quantity);
            }
        }
        private void UpdateProductQuantity(int productId, int quantity)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                product.ProductQuantity -= quantity;
                _context.SaveChanges();
            }
        }

    }
}
