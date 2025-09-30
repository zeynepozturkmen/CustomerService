using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces;
using CustomerService.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace CustomerService.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(Customer customer)
        {
            var existing = await _context.Customers.FindAsync(customer.Id);
            if (existing == null) return null;

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Email = customer.Email;
            existing.PhoneNumber = customer.PhoneNumber;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null) return false;

            _context.Customers.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

