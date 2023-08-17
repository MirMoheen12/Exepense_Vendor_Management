using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exepense_Vendor_Management.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<EmployeeExpense> EmployeeExpense { get; set; }
        public DbSet<CostCenterExpense> CostCenterExpense { get; set; }
        public DbSet<Media> Media { get; set; }
    }
}
