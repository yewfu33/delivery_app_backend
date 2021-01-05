using CsvHelper.Configuration;
using Delivery_app.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class CourierStatementModel
    {
        public int[] settle_list { get; set; }
        public int payment_id { get; set; }
        public int order_id { get; set; }
        public string courier { get; set; }
        public double commission { get; set; }
        public DateTime created_on { get; set; }
        public double order_amount { get; set; }
        public double courier_pay { get; set; }
        public PaymentMethod payment_method { get; set; }
        public CourierPaymentStatus courier_payment_status { get; set; }
    }

    public sealed class CourierStatementModelMap : ClassMap<CourierStatementModel>
    {
        public CourierStatementModelMap()
        {
            Map(m => m.payment_id).Name("#");
            Map(m => m.order_id).Name("Order Id");
            Map(m => m.courier).Name("Courier");
            Map(m => m.commission).Name("Commission");
            Map(m => m.created_on).Name("Created On");
            Map(m => m.order_amount).Name("Order Amount");
            Map(m => m.courier_pay).Name("Courier Pay");
            Map(m => m.payment_method).Name("Payment Method");
            Map(m => m.courier_payment_status).Name("Status");
        }
    }
}
