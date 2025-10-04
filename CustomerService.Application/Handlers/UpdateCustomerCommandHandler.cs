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
            // Payload oluştur
            var payload = new
            {
                SenderId = senderId
            };

            // HttpRequestMessage oluştur
            // Transfer servisi customer blocked duruma düştüğünde bekleyen transferlerin otomatik iptali, base url örneği:
            //https://https://localhost:44378/api/Transfer/customerBlocked
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:44378/api/Transfer/customerBlocked")
            {
                Content = JsonContent.Create(payload)
            };

            // API Key header ekle
            request.Headers.TryAddWithoutValidation("X-Api-Key", "my-secret-api-key-123");

            // İstek gönder
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
