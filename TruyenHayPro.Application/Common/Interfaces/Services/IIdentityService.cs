using System.Security.Claims;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Common.Interfaces.Services;

public interface IIdentityService
{
    Task<Result<Guid>> RegisterUserAsync(string email, string username, string password, string fullName);

    Task<Result<AuthResponse>> LoginAsync(string username, string password);
    
    Guid GetUserId(ClaimsPrincipal user);

    Task<UserInfoDto> GetUserInfoAsync(Guid userId);
}