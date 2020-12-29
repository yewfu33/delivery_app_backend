using AutoMapper;
using Delivery_app.Entities;
using Delivery_app.Services;
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
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public UserController(AppDbContext context, IMapper mapper, IToastNotification toastNotification)
        {
            _context = context;
            _mapper = mapper;
            _toastNotification = toastNotification;
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _context.users.FindAsync(id);

                if (user == null)
                {
                    _toastNotification.AddErrorToastMessage($"Failed to fetch edit user.");
                    return RedirectToAction("Index");
                }

                var userViewModel = _mapper.Map<UserViewModel>(user);

                return View(userViewModel);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                _toastNotification.AddErrorToastMessage($"Failed to edit.");
                return View(model);
            }

            var editUser = _mapper.Map<Users>(model);

            // update fields
            user.name = editUser.name;
            user.phone_num = editUser.phone_num;
            user.email = editUser.email;
            user.locked = editUser.locked;
            user.updated_at = DateTime.Now;

            _context.users.Update(user);

            await _context.SaveChangesAsync();

            _toastNotification.AddInfoToastMessage("Edited successfully.");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ResetPassword(int id)
        {
            try
            {
                ResetPasswordModel model = new ResetPasswordModel();

                var user = await _context.users.FindAsync(id);

                if (user == null)
                {
                    _toastNotification.AddErrorToastMessage($"Failed to fetch user.");
                    return RedirectToAction("Index");
                }

                model.id = user.user_id;

                return View(model);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _context.users.FindAsync(model.id);

                if (user == null)
                {
                    _toastNotification.AddErrorToastMessage($"Failed to fetch user.");
                    return View(model);
                }

                byte[] passwordHash, passwordSalt;

                AccountService.CreatePasswordHash(model.new_password, out passwordHash, out passwordSalt);

                user.password = Convert.ToBase64String(passwordHash);
                user.password_salt = Convert.ToBase64String(passwordSalt);

                _context.users.Update(user);
                await _context.SaveChangesAsync();

                _toastNotification.AddInfoToastMessage("Edited successfully.");

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return View(model);
            }
        }
    }
}
