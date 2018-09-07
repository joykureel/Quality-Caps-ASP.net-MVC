using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityCaps.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "not longer than 15 characters")]
       
        public string Name { get; set; }
       
        [Required]
        [StringLength(50, ErrorMessage = "not longer than 50 characters")]
        public string Description { get; set; }
        public ICollection<Cap> Caps { get; set; }
    }
}
