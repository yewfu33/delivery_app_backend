using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string new_password { get; set; }
        [Required]
        [Compare("new_password", ErrorMessage = "Password is not the same.")]
        public string confirm_password { get; set; }
    }
}
