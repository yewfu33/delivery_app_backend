using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public enum DiscountType : byte
    {
        Percent,
        Value
    }

    public class PromoCodes
    {
        [Key]
        public int promo_code_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double discount { get; set; }
        public DiscountType discount_type { get; set; }
        public double minimum_spend { get; set; }
        public int quantity { get; set; }
        public int num_claim_per_user { get; set; }
        public DateTime validity { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
