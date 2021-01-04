using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string name { get; set; }
        public string password { get; set; }
        public string password_salt { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string phone_num { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string email { get; set; }
        public string fcm_token { get; set; }
        public UserType user_type { get; set; }
        public ICollection<Orders> orders { get; }
        public bool locked { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
