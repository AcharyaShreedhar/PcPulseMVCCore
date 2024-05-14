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
- This controller handles various actions related to the dashboard functionality of the PcPulse application.
- It manages user management, order management, and notification display.
*/

using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PcPulse.Areas.Identity.Data;
using PcPulse.ViewModel;

namespace PcPulse.Controllers
{
    public class DashboardController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PcPulseDbContext _context;
        private readonly INotyfService _Notification;
        public DashboardController(PcPulseDbContext application, UserManager<ApplicationUser> userManager, INotyfService Notification)
        {

            _context = application;
            _userManager = userManager;
            _Notification = Notification;
        }
        public IActionResult DashboardIndex()
        {
            return View();
        }
        public IActionResult Manageuser()
        {
            var obj = (from user in _context.Users
                       join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                       join roles in _context.Roles on userRoles.RoleId equals roles.Id
                       where roles.Name == "User"
                       select new UserViewModel
                       {
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           Email = user.Email,
                           UserRole = roles.Name,
                           userId = user.Id
                       }).ToList();
            return View(obj);
        }
        public IActionResult DeleteUser(string id)
        {
            var getData = _context.Users.Find(id);
            if (getData != null)
            {
                _context.Users.Remove(getData);
                _context.SaveChanges();
                _Notification.Success("User is deleted successfully.");
                return Json(new { success = true, message = "User is deleted successfully." });
            }
            else
            {
                _Notification.Error("Error! Error Deleting User. Please try again.");
                return Json(new { success = false, message = "Error! Deleting User. Please try again." });

            }
        }

        public IActionResult PendingOrder()
        {
            var getOrder = (from order in _context.Orders
                            join od in _context.OrderDetails on order.Id equals od.OrderId
                            where order.OrderType == "Pending"
                            group order by new { order.OrderType, order.OrderDate, order.OrderNumber, order.Id, order.FirstName, order.LastName, order.Email, order.PhoneNumber, order.CountryState, order.Address, order.City } into groupedOrders
                            where groupedOrders.Any()
                            select new OrderViewModel
                            {
                                OrderNumber = groupedOrders.Key.OrderNumber.ToString(),
                                OrderDate = groupedOrders.Key.OrderDate.Date.ToString(),
                                FirstName = groupedOrders.Key.FirstName,
                                LastName = groupedOrders.Key.LastName,
                                CustomerEmail = groupedOrders.Key.Email,
                                MobileNumber = groupedOrders.Key.PhoneNumber,
                                Country = groupedOrders.Key.CountryState,
                                Address = groupedOrders.Key.Address,
                                City = groupedOrders.Key.City,
                                OrderCount = groupedOrders.Count(),
                                orderid = groupedOrders.Key.Id,
                                OrderStatus = groupedOrders.Key.OrderType
                            }).ToList();
            return View(getOrder);
        }
        public IActionResult ClearPendingOrder(int? id)
        {
            var obj = _context.Orders.Where(x => x.Id == id).FirstOrDefault();
            if (obj != null)
            {
                obj.OrderType = "Delivered";
                _context.SaveChanges();

                _Notification.Success("Order is succesfully delivered to user.");
                return RedirectToAction("PendingOrder");

            }
            _Notification.Error("Error! Delivering the Order.Please try again.");

            return NotFound();
        }
        public IActionResult DeliverOrder()
        {

            var obj = (from od in _context.OrderDetails
                       join order in _context.Orders on od.OrderId equals order.Id
                       join pro in _context.Products on od.productId equals pro.Id
                       where order.OrderType == "Deliver"
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
                           Country = order.CountryState,
                           City = order.City,
                       }).ToList();
            return View(obj);
        }
    }

}
