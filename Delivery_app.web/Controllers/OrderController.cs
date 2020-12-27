using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public OrderController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var orders = await _context.orders
                                    .OrderByDescending(o => o.created_at)
                                    .ToListAsync();

            var orderViewModel = _mapper.Map<List<Orders> ,List <OrderViewModel>>(orders);

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
    }
}
