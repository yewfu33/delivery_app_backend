using CsvHelper.Configuration;
using Delivery_app.Entities;
using Delivery_app.Models;
using Delivery_app.web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class OrderReportModel
    {
        public int order_id { get; set; }
        public string username { get; set; }
        public string delivery_item { get; set; }
        public DateTime created_on { get; set; }
        public string courier { get; set; }
        public DeliveryStatus delivery_status { get; set; }
    }

    public sealed class OrderReportModelMap : ClassMap<OrderReportModel>
    {
        public OrderReportModelMap()
        {
            Map(m => m.order_id).Name("#");
            Map(m => m.username).Name("Username");
            Map(m => m.delivery_item).Name("Delivery Item");
            Map(m => m.created_on).Name("Created On");
            Map(m => m.courier).Name("Courier");
            Map(m => m.delivery_status).Name("Delivery Status");
        }
    }
}
