using CustomerService.Application.Commands;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Interfaces;
using MediatR;
using System.Net.Http.Json;


namespace CustomerService.Application.Handlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly HttpClient _httpClient;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, HttpClient httpClient)
        {
            _customerRepository = customerRepository;
            _httpClient = httpClient;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null) return false;

            if (customer.Status != request.Status && request.Status == CustomerStatus.Blocked)
            {
                await CancelPendingTransfersForBlockedCustomerAsync(request.Id);
            }

            customer.Name = request.Name;
            customer.Surname = request.Surname;
            customer.Phone = request.Phone;
            customer.Email = request.Email;
            customer.Status = request.Status;
            await _customerRepository.UpdateAsync(customer);

            return true;
        }

        public async Task CancelPendingTransfersForBlockedCustomerAsync(Guid senderId)
        {

            // Transfer servisi customer blocke) duruma düştüğünde bekleyen transferlerin otomatik iptali, base url örneği:
            //https://https://localhost:44378/api/Transfer/customerBlocked

            var payload = new
            {
                SenderId = senderId
            };

            await _httpClient.PutAsJsonAsync("https://localhost:44378/api/Transfer/customerBlocked", payload);
        }
    }
}
