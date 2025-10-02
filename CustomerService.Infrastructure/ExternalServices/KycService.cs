using CustomerService.Application.Common;
using CustomerService.Infrastructure.Response;
using System.Net.Http.Json;


namespace CustomerService.Infrastructure.ExternalServices
{
    public class KycService : IKycService
    {
        private readonly HttpClient _httpClient;
        public KycService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> VerifyAsync(string userIdNo, string nationalId, string birthYear)
        {
            var payload = new
            {
                userId = userIdNo,
                tcno = nationalId,
                birthYear = birthYear
            };

            // KYC servisi docker container'ında çalışıyor, base url örneği:
            // http://localhost:8082/verify
            var response = await _httpClient.PostAsJsonAsync("http://localhost:8082/api/kyc/verify", payload);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<KycResult>();
            return result?.Verified ?? false;
        }
    }
}
