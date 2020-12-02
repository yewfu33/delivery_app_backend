using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class AdminLoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This field is required")]
        public string email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        public string password { get; set; }

        public bool rememberMe { get; set; }
        public string? validationError { get; set; }
    }
}
