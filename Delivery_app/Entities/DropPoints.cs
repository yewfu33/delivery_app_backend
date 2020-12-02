using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class DropPoints
    {
        [Key]
        public int drop_point_id { get; set; }
        public string address { get; set; }
        [Column(TypeName = "FLOAT(10, 6)")]
        public float latitude { get; set; }

        [Column(TypeName = "FLOAT(10, 6)")]
        public float longitude { get; set; }
        public string contact_num { get; set; }
        public DateTime datetime { get; set; }
        public string comment { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int order_id { get; set; }
    }
}
