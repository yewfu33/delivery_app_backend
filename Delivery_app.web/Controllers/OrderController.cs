using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public OrderController(AppDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var orders = await _context.orders
                                    .OrderByDescending(o => o.created_at)
                                    .ToListAsync();

            var orderViewModel = _mapper.Map<List<Orders>, List <OrderViewModel>>(orders);

            if (!String.IsNullOrEmpty(searchQuery))
            {
                orderViewModel = orderViewModel.Where(o =>
                    o.name.Contains(searchQuery)
                    || o.contact_num.Contains(searchQuery)
                    || o.pick_up_address.Contains(searchQuery)
                    ).ToList();
            }

            return View(orderViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            try
            {
                var order = await _context.orders.FindAsync(id);

                if (order == null)
                {
                    _toastNotification.AddErrorToastMessage($"Order no found.");
                    return RedirectToAction("Index");
                }

                if (order.delivery_status != DeliveryStatus.Assigned)
                {                    
                    _toastNotification.AddErrorToastMessage($"In delivery or completed order cannot be cancel.");
                    return RedirectToAction("Index");
                }

                // othherwise set order cancelled
                order.delivery_status = DeliveryStatus.Cancelled;

                _context.orders.Update(order);
                await _context.SaveChangesAsync();

                _toastNotification.AddInfoToastMessage("Cancelled order successfully.");

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return RedirectToAction("Index");
            }
        }
    }
}
