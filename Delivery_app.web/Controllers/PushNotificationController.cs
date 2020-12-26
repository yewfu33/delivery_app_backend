using Delivery_app.Entities;
using Delivery_app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class PushNotificationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly INotificationService _notificationService;

        public PushNotificationController(AppDbContext context,
            IToastNotification toastNotification,
            INotificationService notificationService)
        {
            _context = context;
            _toastNotification = toastNotification;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                PushNotificationModel model = new PushNotificationModel();

                var users = await _context.users
                                    .Select(u => new SelectListItem
                                    {
                                        Value = u.user_id.ToString(),
                                        Text = u.name
                                    })
                                    .ToListAsync();

                model.userList.AddRange(users);

                return View(model);
            }
            catch (Exception)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong, try again later.");

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(PushNotificationModel model)
        {
            if (!ModelState.IsValid)
            {
                var users = await _context.users
                                    .Select(u => new SelectListItem
                                    {
                                        Value = u.user_id.ToString(),
                                        Text = u.name
                                    })
                                    .ToListAsync();

                model.userList.AddRange(users);

                return View(model);
            }

            try
            {
                // send notification to selected users
                List<Task> tasks = new List<Task>();

                foreach (var u in model.toUser)
                {
                    Task t = Task.Run(() => sendPushNotificationHandler(u, model.title, model.content));

                    tasks.Add(t);
                }

                Task.WaitAll(tasks.ToArray());

                _toastNotification.AddSuccessToastMessage("Push notification sent successfully.");

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went wrong, try again later.");

                return RedirectToAction("Index");
            }
        }

        private async Task sendPushNotificationHandler(string uid, string title, string content)
        {
            var user = await _context.users.FindAsync(int.Parse(uid));
            await _notificationService
                        .SendNotification(user.fcm_token, title, title, content);
        }
    }
}
