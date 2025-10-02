using CustomerService.Domain.Entities;
using MediatR;


namespace CustomerService.Application.Queries
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }

        public GetCustomerByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
