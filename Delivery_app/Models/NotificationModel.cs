using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class NotificationModel 
    {
        [Required]
        public string fcmToken { get; set; }
        [Required]
        public string collapseKey { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string body { get; set; }
    }
}
