using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Models
{
    public class Supplier
    {
        public int ID { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
      
        public string Name { get; set; }
        [Required]
      
        public int Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Cap> Caps {get;set;}
        
    }
}
