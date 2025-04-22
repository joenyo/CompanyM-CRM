using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyM_CRM.Models
{
    public class Case
    {
        [Key]
        public int CaseID { get; set; }
        public int CustomerID { get; set; }
        public int ChannelID { get; set; }
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public string Status { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDate { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual ChannelType? Channel { get; set; }
    }
}