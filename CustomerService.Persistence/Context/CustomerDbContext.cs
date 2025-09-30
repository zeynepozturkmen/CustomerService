using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace CustomerService.Persistence.Context
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
