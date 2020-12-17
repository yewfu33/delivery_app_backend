using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class UserModel
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string phone_num { get; set; }
        public string email { get; set; }
        public int user_type { get; set; }
        public string fcm_token { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
