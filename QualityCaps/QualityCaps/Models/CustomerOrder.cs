using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Models
{
    public class CustomerOrder
    {
        public int ID { get; set; }
        public string Status { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Gst { get; set; }
        public Order Order { get; set; }
    }
}
