/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- PcPulseDbContext: Manages database interactions for the Electronics Store application, including CRUD operations and schema management.
- Products: Represents products available in the store.
- Categories: Represents categories to which products belong.
- Orders: Stores information about orders placed by customers.
- OrderDetails: Contains detailed information about products included in each order.
*/



#nullable disable
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PcPulse.Models;

namespace PcPulse.Areas.Identity.Data;

public class PcPulseDbContext : IdentityDbContext<ApplicationUser>
{
    public PcPulseDbContext(DbContextOptions<PcPulseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}
