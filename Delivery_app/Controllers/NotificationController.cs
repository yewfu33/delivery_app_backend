using Delivery_app.Models;
using Delivery_app.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Index(NotificationModel notificationModel)
        {
            try
            {
                await _notificationService.sendNotification(notificationModel.fcmToken, notificationModel.collapseKey,
                    notificationModel.title, notificationModel.body);

                return Ok();
            }catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = e.Message });
            }
        }
    }
}
