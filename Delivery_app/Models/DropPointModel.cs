using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class DropPointModel
    {
        public int drop_point_id { get; set; }
        public string address { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string contact_num { get; set; }
        public DateTime datetime { get; set; }
        public string comment { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int order_id { get; set; }
    }
}
