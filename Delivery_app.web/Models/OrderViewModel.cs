using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class OrderViewModel
    {
        public int order_id { get; set; }
        public string name { get; set; }
        public string pick_up_address { get; set; }
        public double weight { get; set; }
        public string contact_num { get; set; }
        public DateTime pick_up_datetime { get; set; }
        public double price { get; set; }
        public DeliveryStatus delivery_status { get; set; }
        public int user_id { get; set; }
    }
}
