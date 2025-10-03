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
            if (!ValidateTurkishIdentityNumber(request.NationalId))
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

        public static bool ValidateTurkishIdentityNumber(string identityNumber)
        {
            // Uzunluk ve null kontrolü
            if (string.IsNullOrWhiteSpace(identityNumber) || identityNumber.Length != 11)
                return false;

            // Rakamdan oluşmalı
            foreach (char c in identityNumber)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            // İlk hanesi 0 olamaz
            if (identityNumber[0] == '0')
                return false;

            // Karakterleri tek tek sayıya dönüştürelim
            int[] digits = new int[11];
            for (int i = 0; i < 11; i++)
            {
                digits[i] = int.Parse(identityNumber[i].ToString());
            }

            // 1,3,5,7,9 toplamı
            int oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            // 2,4,6,8 toplamı
            int evenSum = digits[1] + digits[3] + digits[5] + digits[7];

            // 10. hane kontrolü
            int checkDigit10 = ((oddSum * 7) - evenSum) % 10;
            if (checkDigit10 != digits[9])
                return false;

            // 11. hane kontrolü
            int checkDigit11 = (digits[0] + digits[1] + digits[2] + digits[3] + digits[4] +
                                digits[5] + digits[6] + digits[7] + digits[8] + digits[9]) % 10;

            if (checkDigit11 != digits[10])
                return false;

            return true;
        }

    }
}
