using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class EmailModel
    {
        public EmailModel()
        {
            emailList = new List<SelectListItem>();
        }

        [Required(ErrorMessage = "The to field is required.")]
        public List<string> toEmail { get; set; }
        public List<SelectListItem> emailList { get; set; }
        [Required]
        public string subject { get; set; }
        [Required]
        public string body { get; set; }
    }
}
