using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Cap Cap { get; set; }
        public Order Order { get; set; }

    }
}
