using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyM_CRM.Models
{
    public class ChannelType
    {
        public ChannelType()
        {
            Cases = new List<Case>();
        }

        [Key]
        public int ChannelID { get; set; }
        
        [Required]
        public string ChannelName { get; set; } = string.Empty;
        
        public virtual ICollection<Case> Cases { get; set; }
    }
}