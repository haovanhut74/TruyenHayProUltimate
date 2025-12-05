using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Common.Interfaces.Services;

public interface IIdentityService
{
    Task<Result<Guid>> RegisterUserAsync(string email, string username, string password, string fullName);
}