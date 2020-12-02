using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class RegisterModel
    {
        [Required]
        public String name { get; set; }
        [Required]
        public String password { get; set; }
        [Required]
        public String phone_num { get; set; }
        [Required]
        public int user_type { get; set; }
    }
}
