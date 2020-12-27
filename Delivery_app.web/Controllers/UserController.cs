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
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var users = await _context.users.ToListAsync();

            var userViewModel = _mapper.Map<List<Users>, List<UserViewModel>>(users);

            if (!String.IsNullOrEmpty(searchQuery))
            {
                userViewModel = userViewModel.Where(u =>
                    u.name.Contains(searchQuery)
                    || u.phone_num.ToString().Contains(searchQuery)
                    ).ToList();
            }

            return View(userViewModel);
        }
    }
}
