using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class Couriers
    {
        public Couriers()
        {
            this.documents = new List<Documents>();
        }

        [Key]
        public int courier_id { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string name { get; set; }
        public string otp { get; set; }
        public bool? onBoard { get; set; }
        public string password { get; set; }
        public string password_salt { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string phone_num { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        public string email { get; set; }

        [Column(TypeName = "VARCHAR(10)")]
        public string vehicle_plate_no { get; set; }

        public string profile_picture { get; set; }
        public ICollection<Documents> documents { get; set; }
        public VehicleType vehicle_type { get; set; }
        public double commission { get; set; }
        public double revenue { get; set; }
        public bool disable { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
