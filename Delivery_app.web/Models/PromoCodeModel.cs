using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class PromoCodeModel
    {
        [Required]
        public int promo_code_id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public double discount { get; set; }
        public DiscountType discount_type { get; set; } = DiscountType.Percent;
        [Required]
        public double minimum_spend { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public int num_claim_per_user { get; set; }
        [Required]
        public DateTime validity { get; set; } = DateTime.Now;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
