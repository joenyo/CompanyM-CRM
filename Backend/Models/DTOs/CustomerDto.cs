namespace CompanyM_CRM.Models.DTOs
{
    public class CustomerDto
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateAdded { get; set; }
    }
}