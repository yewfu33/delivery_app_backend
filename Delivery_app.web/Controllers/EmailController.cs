using Delivery_app.Entities;
using Delivery_app.web.Models;
using Delivery_app.web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Controllers
{
    public class EmailController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IEmailService _emailService;

        public EmailController(AppDbContext context, 
            IToastNotification toastNotification,
            IEmailService emailService)
        {
            _context = context;
            _toastNotification = toastNotification;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            EmailModel model = new EmailModel();

            var userGroup = new SelectListGroup { Name = "Users" };
            var courierGroup = new SelectListGroup { Name = "Couriers" };

            var userEmail = await _context.users
                                .Where(u => u.email != null)
                                .Select(u => new SelectListItem
                                    {
                                        Value = u.email,
                                        Text = u.email,
                                        Group = userGroup
                                    })
                                .ToListAsync();

            var courierEmail = await _context.couriers
                                .Where(c => c.email != null)
                                .Select(c => new SelectListItem
                                {
                                    Value = c.email,
                                    Text = c.email,
                                    Group = courierGroup
                                })
                                .ToListAsync();

            model.emailList.AddRange(userEmail);
            model.emailList.AddRange(courierEmail);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailModel model)
        {
            if (!ModelState.IsValid)
            {
                var userGroup = new SelectListGroup { Name = "Users" };
                var courierGroup = new SelectListGroup { Name = "Couriers" };

                var userEmail = await _context.users
                                    .Where(u => u.email != null)
                                    .Select(u => new SelectListItem
                                    {
                                        Value = u.email,
                                        Text = u.email,
                                        Group = userGroup
                                    })
                                    .ToListAsync();

                var courierEmail = await _context.couriers
                                    .Where(c => c.email != null)
                                    .Select(c => new SelectListItem
                                    {
                                        Value = c.email,
                                        Text = c.email,
                                        Group = courierGroup
                                    })
                                    .ToListAsync();

                model.emailList.AddRange(userEmail);
                model.emailList.AddRange(courierEmail);

                return View(model);
            }

            try
            {
                // send email to selected emails
                await _emailService.SendEmailAsync(
                        "Hi",
                        model.toEmail,
                        model.subject,
                        model.body
                    );

                _toastNotification.AddSuccessToastMessage("Email sent successfully.");

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong, try again later.");

                return RedirectToAction("Index");
            }
        }
    }
}
