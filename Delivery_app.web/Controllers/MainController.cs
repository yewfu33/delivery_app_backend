using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Delivery_app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    }
}
