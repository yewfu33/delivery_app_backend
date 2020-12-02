using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class CourierRegisterModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string phone_num { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string vehicle_plate_no { get; set; }
        public string profile_picture { get; set; }
        public List<DocumentModel> documents { get; set; }
        public string profile_picture_name { get; set; }
        [Required]
        public int vehicle_type { get; set; }
    }
}
