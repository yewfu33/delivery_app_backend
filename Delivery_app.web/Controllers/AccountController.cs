using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Delivery_app.Entities;
using Delivery_app.Services;
using Delivery_app.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Delivery_app.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Delivery_app.web.Controllers
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginModel model, [FromQuery] string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = await this.Authenticate(model);

            if (admin == null)
            {
                model.password = "";
                model.validationError = "Invalid login";
                return View(model);
            }

            //claims authentication
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, admin.email)
             };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = model.rememberMe,
                });

            //redirect to return url if any
            if (Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Main");
        }

        [HttpPost]
        public IActionResult Register([FromBody] Admin model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            byte[] passwordHash, passwordSalt;

            AccountService.CreatePasswordHash(model.password, out passwordHash, out passwordSalt);

            model.password = Convert.ToBase64String(passwordHash);
            model.password_salt = Convert.ToBase64String(passwordSalt);
            model.created_at = DateTime.Now;
            model.updated_at = DateTime.Now;

            _context.admins.Add(model);
            _context.SaveChanges();

            return Ok(new Admin { 
                admin_id = model.admin_id,
                name = model.name,
                email = model.email,
                created_at = model.created_at,
                updated_at = model.updated_at,
            });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        private async Task<Admin> Authenticate(AdminLoginModel model)
        {
            if (string.IsNullOrEmpty(model.email) || string.IsNullOrEmpty(model.password))
                return null;

            var admin = await _context.admins.SingleOrDefaultAsync(x => x.email == model.email);

            // check if user exists
            if (admin == null) return null;

            // check password is correct
            if (!AccountService.VerifyPasswordHash(model.password, Convert.FromBase64String(admin.password), Convert.FromBase64String(admin.password_salt)))
                return null;

            return admin;
        }
    }
}
