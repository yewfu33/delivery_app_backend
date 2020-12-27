using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.web.Models
{
    public class DashboardViewModel
    {
        public int totalOrdersCount { get; set; }
        public int totalCouriersCount { get; set; }
        public int totalUsersCount { get; set; }
        public int totalIncome { get; set; }
    }
}
