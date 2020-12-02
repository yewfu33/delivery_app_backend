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

namespace Delivery_app.web.Controllers
{
    public class CourierController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly MailSettings _mailSettings;
        private Random random = new Random();

        public CourierController(AppDbContext context, 
            IOptions<MailSettings> mailSettings, 
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _mailSettings = mailSettings.Value;
        }

        public async Task<IActionResult> Index()
        {
            var CouriersList = await _context.couriers.Select(c => 
                new Couriers{
                    courier_id = c.courier_id,
                    profile_picture = c.profile_picture,
                    name = c.name,
                    email = c.email,
                    phone_num = c.phone_num,
                    vehicle_plate_no = c.vehicle_plate_no,
                    vehicle_type = c.vehicle_type,
                    documents = c.documents,
                    created_at = c.created_at,
                    updated_at = c.updated_at
                }
            ).ToListAsync();

            return View(CouriersList);
        }

        public async Task<IActionResult> Accept(int cid)
        {
            try
            {
                var courier = await _context.couriers.FindAsync(cid);

                if (courier == null) return View("Index");

                var otp = this.RandomString(6);

                await _emailService.SendEmailAsync(
                    "Dear Applicant",
                    courier.email,
                    "Your Couriers Registration have been Approved",
                    "Your couriers registration on Delivery App have been approved.<br/>" +
                    "kindly login into with the following otp (one time password),<br/>" +
                    $"<h2>{otp}</h2><br/>" +
                    "you be able to change your password afterward successful login in."
                );

                courier.otp = otp;
                _context.couriers.Update(courier);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Index");
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
