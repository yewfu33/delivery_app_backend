using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class CourierModel
    {
        public int courier_id { get; set; }
        public string name { get; set; }
        public string phone_num { get; set; }
        public string profile_picture { get; set; }
    }
}
