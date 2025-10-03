using CustomerService.Application.Commands;
using CustomerService.Application.Common;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces;
using MediatR;


namespace CustomerService.Application.Handlers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IKycService _kyc;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IKycService kyc)
        {
            _customerRepository = customerRepository;
            _kyc = kyc;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            // Age rule
            if ((DateTime.UtcNow.Year - request.DateOfBirth.Year) < 18)
                throw new Exception("Customer must be 18+");

            // National ID validation
            if (request.NationalId.Length != 11)
                throw new Exception("Invalid National ID");

            if (request.CustomerType == CustomerType.Corporate && String.IsNullOrEmpty(request.TaxNumber))
                throw new Exception("The tax number is mandatory for corporate customers");

            var userIdNo = Guid.NewGuid();

            // Call KYC external service
            var isValid = await _kyc.VerifyAsync(userIdNo.ToString(), request.NationalId, request.DateOfBirth.ToString("yyyy"));
            if (!isValid)
                throw new Exception("KYC verification failed");

            var customer = new Customer
            {
                Id = userIdNo,
                Name = request.Name,
                Surname = request.Surname,
                NationalId = request.NationalId,
                Phone = request.Phone,
                DateOfBirth = request.DateOfBirth,
                Type = request.CustomerType,
                TaxNumber = request.TaxNumber,
                Status = CustomerStatus.Active,
                Email = request.Email,
            };

            await _customerRepository.AddAsync(customer);
           
            return customer;
        }
    }
}
