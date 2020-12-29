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
using AutoMapper;

namespace Delivery_app.web.Controllers
{
    public class CourierController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly MailSettings _mailSettings;
        private Random random = new Random();

        public CourierController(AppDbContext context, 
            IOptions<MailSettings> mailSettings, 
            IEmailService emailService,
            IMapper mapper,
            IToastNotification toastNotification)
        {
            _context = context;
            _emailService = emailService;
            _mapper = mapper;
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
                        disable = c.disable,
                        commission = c.commission,
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
                        disable = c.disable,
                        commission = c.commission,
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

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var courier = await _context.couriers.FindAsync(id);

                if(courier == null)
                {
                    _toastNotification.AddErrorToastMessage($"Failed to fetch edit courier.");
                    return RedirectToAction("Index");
                }

                var courierViewModel = _mapper.Map<CourierViewModel>(courier);

                return View(courierViewModel);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CourierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var courier = await _context.couriers.FindAsync(id);

            if (courier == null)
            {
                _toastNotification.AddErrorToastMessage($"Failed to edit.");
                return View(model);
            }

            var editCourier = _mapper.Map<Couriers>(model);

            // update fields
            courier.name = editCourier.name;
            courier.phone_num = editCourier.phone_num;
            courier.email = editCourier.email;
            courier.vehicle_plate_no = editCourier.vehicle_plate_no;
            courier.vehicle_type = editCourier.vehicle_type;
            courier.commission = editCourier.commission;
            courier.disable = editCourier.disable;
            courier.updated_at = DateTime.Now;

            _context.couriers.Update(courier);

            await _context.SaveChangesAsync();

            _toastNotification.AddInfoToastMessage("Edited successfully.");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AcceptRegistration(int id)
        {
            try
            {
                var courier = await _context.couriers.FindAsync(id);

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
                _toastNotification.AddErrorToastMessage($"Error occured, try again later.");
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
