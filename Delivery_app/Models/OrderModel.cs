using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class OrderModel
    {
        public int order_id { get; set; }
        public string name { get; set; }
        public string pick_up_address { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public double weight { get; set; }
        public string comment { get; set; }
        public string contact_num { get; set; }
        public DateTime pick_up_datetime { get; set; }
        public double price { get; set; }
        public int delivery_status { get; set; }
        public int vehicle_type { get; set; }
        public bool notify_sender { get; set; }
        public bool notify_recipient { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int user_id { get; set; }
        public UserModel user { get; set; }
        public ICollection<DropPointModel> drop_points { get; set; }
    }
}
