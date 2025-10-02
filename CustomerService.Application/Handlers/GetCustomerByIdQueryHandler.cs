using CustomerService.Application.Queries;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces;
using MediatR;


namespace CustomerService.Application.Handlers
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
                throw new Exception($"Customer with Id {request.Id} not found");

            return customer;
        }
    }
}
