using CustomerService.Domain.Entities;
using MediatR;


namespace CustomerService.Application.Commands
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }  // Güncellenecek müşteri id’si
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public CustomerStatus Status { get; set; }
    }
}
