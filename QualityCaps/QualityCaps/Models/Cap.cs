using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace QualityCaps.Models
{
    public class Cap
    {
        public int ID { get; set; }
        [Required]
        
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string Name { get; set; }
       

        [StringLength(50, ErrorMessage = "not longer than 50 characters")]

        public string Description { get; set; }
        [Required]
      
        public Decimal Price { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public string Image { get; set; }
        public Supplier Supplier { get; set; }
        public Category Category { get; set; }
    }
}
