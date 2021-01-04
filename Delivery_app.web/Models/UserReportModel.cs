using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class UserReportModel
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string phone_num { get; set; }
        public string email { get; set; }
        public int total_orders { get; set; }
        public DateTime created_at { get; set; }
    }

    public sealed class UserReportModelMap : ClassMap<UserReportModel>
    {
        public UserReportModelMap()
        {
            Map(m => m.user_id).Name("#");
            Map(m => m.name).Name("Name");
            Map(m => m.phone_num).Name("Phone Num");
            Map(m => m.email).Name("Email");
            Map(m => m.total_orders).Name("Total Orders");
            Map(m => m.created_at).Name("Created At");
        }
    }
}
