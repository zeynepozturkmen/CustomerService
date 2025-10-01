using CustomerService.Application.Common;
using System.Net.Http.Json;


namespace CustomerService.Infrastructure.ExternalServices
{
    public class KycService : IKycService
    {
        private readonly HttpClient _http;
        public KycService(HttpClient http) => _http = http;

        public async Task<bool> VerifyAsync(string nationalId)
        {
            //var response = await _http.PostAsJsonAsync("/verify", new { NationalId = nationalId });
            //if (!response.IsSuccessStatusCode) return false;

            //var result = await response.Content.ReadFromJsonAsync<KycResponse>();

           //return result?.IsValid ?? false;

            return true;
        }

        private record KycResponse(bool IsValid);
    }
}
