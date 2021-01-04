using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Models
{
    public class ApplyPromoCodeModel
    {
        [Required]
        public double order_fee { get; set; }
        [Required]
        public string promo_code { get; set; }
        [Required]
        public int user_id { get; set; }
    }
}
