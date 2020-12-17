using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class TakeOrderModel
    {
        [Required]
        public int order_id { get; set; }
        [Required]
        public int courier_id { get; set; }
    }
}
