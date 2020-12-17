using Delivery_app.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class Orders
    {

        [Key]
        public int order_id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string name { get; set; }
        public string pick_up_address { get; set; }

        [Column(TypeName = "FLOAT(10, 6)")]
        public float latitude { get; set; }

        [Column(TypeName = "FLOAT(10, 6)")]
        public float longitude { get; set; }
        public double weight { get; set; }
        public string comment { get; set; }
        public string contact_num { get; set; }
        public DateTime pick_up_datetime { get; set; }
        public double price { get; set; }
        public DeliveryStatus delivery_status { get; set; }
        public VehicleType vehicle_type { get; set; }
        public bool notify_sender { get; set; }
        public bool notify_recipient { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int user_id { get; set; }
        public int courier_id { get; set; }
        public Users user { get; }
        public ICollection<DropPoints> drop_points { get; set; }
    }
}
