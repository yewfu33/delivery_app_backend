using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery_app.Entities
{
    public class Documents
    {
        [Key]
        public int document_id { get; set; }
        public string name { get; set; }
        public string document { get; set; }
        public int courier_id { get; set; }
    }
}
