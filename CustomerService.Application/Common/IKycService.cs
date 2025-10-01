
namespace CustomerService.Application.Common
{
    public interface IKycService
    {
        Task<bool> VerifyAsync(string nationalId);
    }
}
