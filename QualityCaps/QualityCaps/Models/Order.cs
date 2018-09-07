using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
      
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string FirstName { get; set; }
        [Required]
       
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string LastName { get; set; }
        
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string City { get; set; }
       
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string State { get; set; }
    
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string PostalCode { get; set; }
      
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string Country { get; set; }
      
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
        public string Phone { get; set; }
        public string OrderStatus { get; set; }
        public decimal Gst { get; set; }
        public decimal Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public ApplicationUser User { get; set; }

    }
}
