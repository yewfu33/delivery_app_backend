using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class CourierReportModel
    {
        public int courier_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public int total_deliver { get; set; }
        public double revenue { get; set; }
    }

    public sealed class CourierReportModelMap : ClassMap<CourierReportModel>
    {
        public CourierReportModelMap()
        {
            Map(m => m.courier_id).Name("#");
            Map(m => m.name).Name("Name");
            Map(m => m.email).Name("Email");
            Map(m => m.total_deliver).Name("Total Deliver");
            Map(m => m.revenue).Name("Revenue");
        }
    }
}
