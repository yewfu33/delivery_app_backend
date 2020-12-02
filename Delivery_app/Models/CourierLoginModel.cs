using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class CourierLoginModel
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
