using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class PushNotificationModel
    {
        public PushNotificationModel()
        {
            userList = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "The to field is required.")]
        public List<string> toUser { get; set; }
        public List<SelectListItem> userList { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string content { get; set; }
    }
}
