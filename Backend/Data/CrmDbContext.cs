using Microsoft.EntityFrameworkCore;
using CompanyM_CRM.Models;

namespace CompanyM_CRM.Data
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ChannelType> ChannelTypes { get; set; }
        public DbSet<Case> Cases { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Case>()
                .HasOne(c => c.Customer)
                .WithMany(c => c.Cases)
                .HasForeignKey(c => c.CustomerID);
                
            modelBuilder.Entity<Case>()
                .HasOne(c => c.Channel)
                .WithMany(c => c.Cases)
                .HasForeignKey(c => c.ChannelID);
                
            // Seed channel types
            modelBuilder.Entity<ChannelType>().HasData(
                new ChannelType { ChannelID = 1, ChannelName = "Visit" },
                new ChannelType { ChannelID = 2, ChannelName = "AI" },
                new ChannelType { ChannelID = 3, ChannelName = "Call" },
                new ChannelType { ChannelID = 4, ChannelName = "WhatsApp" },
                new ChannelType { ChannelID = 5, ChannelName = "Email" }
            );
        }
    }
}