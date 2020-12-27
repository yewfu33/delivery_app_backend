using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Delivery_app.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Delivery_app.web.Settings;
using Microsoft.Extensions.Options;
using Delivery_app.web.Services;
using NToastNotify;
using Delivery_app.web.Models;

namespace Delivery_app.web.Controllers
{
    public class CourierController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IToastNotification _toastNotification;
        private readonly MailSettings _mailSettings;
        private Random random = new Random();

        public CourierController(AppDbContext context, 
            IOptions<MailSettings> mailSettings, 
            IEmailService emailService,
            IToastNotification toastNotification)
        {
            _context = context;
            _emailService = emailService;
            _toastNotification = toastNotification;
            _mailSettings = mailSettings.Value;
        }

        public async Task<IActionResult> Index([FromQuery] string status, string searchQuery)
        {
            List<CourierViewModel> CouriersList;

            if (status == "pending")
            {
                CouriersList = await _context.couriers
                .Where(c => c.otp == null)
                .Select(c =>
                    new CourierViewModel
                    {
                        courier_id = c.courier_id,
                        profile_picture = c.profile_picture,
                        name = c.name,
                        email = c.email,
                        phone_num = c.phone_num,
                        vehicle_plate_no = c.vehicle_plate_no,
                        vehicle_type = c.vehicle_type,
                        documents = c.documents,
                        created_at = c.created_at,
                        updated_at = c.updated_at,
                        isRegistered = false
                    }
                ).ToListAsync();
            }
            else
            {
                CouriersList = await _context.couriers
                .Where(c => c.otp != null)
                .Select(c =>
                    new CourierViewModel
                    {
                        courier_id = c.courier_id,
                        profile_picture = c.profile_picture,
                        name = c.name,
                        email = c.email,
                        phone_num = c.phone_num,
                        vehicle_plate_no = c.vehicle_plate_no,
                        vehicle_type = c.vehicle_type,
                        documents = c.documents,
                        created_at = c.created_at,
                        updated_at = c.updated_at,
                        isRegistered = true
                    }
                ).ToListAsync();
            }

            if (!String.IsNullOrEmpty(searchQuery))
            {
                CouriersList = CouriersList.Where(c =>
                    c.name.Contains(searchQuery)
                    || c.email.Contains(searchQuery)
                    || c.vehicle_plate_no.Contains(searchQuery)
                    ).ToList();
            }

            return View(CouriersList);
        }

        public async Task<IActionResult> AcceptRegistration(int cid)
        {
            try
            {
                var courier = await _context.couriers.FindAsync(cid);

                if (courier == null)
                {
                    _toastNotification.AddErrorToastMessage("Courier no found.");
                    return RedirectToAction("Index");
                }

                if (courier.otp != null)
                {
                    _toastNotification.AddErrorToastMessage("Courier registration already accepted.");
                    return RedirectToAction("Index");
                }

                var otp = this.RandomString(6);

                var toEmail = new List<String>
                {
                    courier.email
                };

                await _emailService.SendEmailAsync(
                    "Dear Applicant",
                    toEmail,
                    "Your Couriers Registration have been Approved",
                    "Your couriers registration on Delivery App have been approved.<br/>" +
                    "kindly login into with the following otp (one time password),<br/>" +
                    $"<h2>{otp}</h2><br/>" +
                    "you be able to change your password afterward successful login in."
                );

                courier.otp = otp;
                _context.couriers.Update(courier);

                await _context.SaveChangesAsync();

                _toastNotification.AddInfoToastMessage("Courier registration approved.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage($"Server error occured: ${ex.Message}");
                return RedirectToAction("Index");
            }

        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
