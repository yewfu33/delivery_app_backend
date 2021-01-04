using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class AddPaymentModel
    {
        [Required]
        public double amount { get; set; }
        [Required]
        public double courier_pay { get; set; }
        [Required]
        public int order_id { get; set; }
        [Required]
        public int user_id { get; set; }
        [Required]
        public int courier_id { get; set; }
    }
}
