using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services.Interface;

public interface IAuthService
{
    Task<Result<Guid>> RegisterAsync(RegisterRequest request);
}