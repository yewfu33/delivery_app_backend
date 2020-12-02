using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class LoginModel
    {
        [Required]
        public String phone_num { get; set; }
        [Required]
        public String password { get; set; }
    }
}
