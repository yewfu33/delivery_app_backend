using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class CourierViewModel
    {
        public CourierViewModel()
        {
            this.documents = new List<Documents>();
        }

        public int courier_id { get; set; }
        public string name { get; set; }
        public string phone_num { get; set; }
        public string email { get; set; }
        public string vehicle_plate_no { get; set; }
        public string profile_picture { get; set; }
        public ICollection<Documents> documents { get; set; }
        public VehicleType vehicle_type { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool isRegistered { get; set; }
    }
}
