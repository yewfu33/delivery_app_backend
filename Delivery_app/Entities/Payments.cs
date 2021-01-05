using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public enum PaymentMethod : byte
    {
        [Display(Name = "Cash")]
        Cash,
        [Display(Name = "Prepay")]
        Prepay
    }

    public enum TransactionStatus : byte
    {
        [Display(Name = "Unsettle")]
        Unsettled,
        [Display(Name = "Received")]
        Received
    }

    public enum CourierPaymentStatus : byte
    {
        [Display(Name = "Unsettle")]
        Unsettled,
        [Display(Name = "Paid")]
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
        public PaymentMethod payment_method { get; set; } = PaymentMethod.Cash;
        public TransactionStatus transaction_status { get; set; } = TransactionStatus.Unsettled;
        public CourierPaymentStatus courier_payment_status { get; set; } = CourierPaymentStatus.Unsettled;
        public DateTime created_at { get; set; }
    }
}
