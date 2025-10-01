using CustomerService.Domain.Entities;
using MediatR;

namespace CustomerService.Application.Commands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public CustomerType Type { get; set; }
    }
}
