using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class Admin
    {
        [Key]
        public int admin_id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Column(TypeName = "VARCHAR(25)")]
        public string email { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Column(TypeName = "VARCHAR(25)")]
        public string name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
        public string password_salt { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
