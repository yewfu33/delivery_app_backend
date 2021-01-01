using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public enum PaymentMethod : byte
    {
        Cash,
        Prepay
    }

    public enum CourierPaymentStatus : byte
    {
        Unsettled,
        Paid
    }

    public class Payments
    {
        [Key]
        public int payment_id { get; set; }
        public double amount { get; set; }
        public double courier_pay { get; set; }
        public int order_id { get; set; }
        public Orders order { get; set; }
        public int user_id { get; set; }
        public Users user { get; set; }
        public int courier_id { get; set; }
        public Couriers courier { get; set; }
        public PaymentMethod payment_method { get; set; }
        public CourierPaymentStatus courier_payment_status { get; set; }
        public DateTime created_at { get; set; }
    }
}
