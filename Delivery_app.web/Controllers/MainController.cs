using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using Delivery_app.Entities;
using Delivery_app.web.Models;
using Microsoft.EntityFrameworkCore;

namespace Delivery_app.web.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly IToastNotification _toastNotification;
        private readonly AppDbContext _context;

        public MainController(ILogger<MainController> logger, 
            IToastNotification toastNotification,
            AppDbContext context)
        {
            _logger = logger;
            _toastNotification = toastNotification;
            _context = context;
        }

        public async Task<IActionResult> Index(DashboardViewModel model)
        {
            try
            {
                IQueryable<Orders> orders = _context.orders;
                model.totalOrdersCount = await orders.CountAsync();

                IQueryable<Users> users = _context.users;
                model.totalUsersCount = await users.CountAsync();

                IQueryable<Couriers> couriers = _context.couriers;
                model.totalCouriersCount = await couriers.CountAsync();

                return View(model);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Failed to load the display data.");

                return View(new DashboardViewModel());
            }
        }

        public async Task<IActionResult> MonthlyOrders()
        {
            try
            {
                var date = DateTime.Now;

                var monthlydata = await _context.orders
                                    .Where(o => o.created_at.Month == date.Month)
                                    .GroupBy(g => new { g.created_at.Day })
                                    .Select(o =>
                                        new
                                        {
                                            day = o.Key.Day,
                                            count = o.Count()
                                        }
                                    )
                                    .ToListAsync();

                return Ok(monthlydata);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
