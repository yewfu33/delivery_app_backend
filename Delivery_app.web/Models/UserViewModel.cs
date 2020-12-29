using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class UserViewModel
    {
        public int user_id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string phone_num { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public bool locked { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
