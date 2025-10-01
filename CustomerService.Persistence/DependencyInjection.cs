using CustomerService.Domain.Interfaces;
using CustomerService.Persistence.Context;
using CustomerService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace CustomerService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<CustomerDbContext>(opt =>
                opt.UseInMemoryDatabase("MoneyBeeDb")); // Demo için InMemory

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
