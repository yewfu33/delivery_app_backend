using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string name { get; set; }
        [Required]
        public string phone_num { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string vehicle_plate_no { get; set; }
        public string profile_picture { get; set; }
        public ICollection<Documents> documents { get; set; }
        [Required]
        public VehicleType vehicle_type { get; set; }
        [Required]
        public double commission { get; set; }
        [Required]
        public bool disable { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool isRegistered { get; set; }
    }
}
