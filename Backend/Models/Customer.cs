using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyM_CRM.Models
{
    public class Customer
    {
        public Customer()
        {
            Cases = new List<Case>();
        }

        [Key]
        public int CustomerID { get; set; }
        
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        
        public virtual ICollection<Case> Cases { get; set; }
    }
}