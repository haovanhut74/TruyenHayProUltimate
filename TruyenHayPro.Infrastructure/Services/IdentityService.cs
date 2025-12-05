using Microsoft.AspNetCore.Identity;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Shared.Wrapper;

// Trỏ về User entity của anh

namespace TruyenHayPro.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Guid>> RegisterUserAsync(string email, string username, string password, string fullName)
    {
        var user = new ApplicationUser
        {
            UserName = username,
            FullName = fullName,
            Email = email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            return Result<Guid>.Success(user.Id);
        }

        // Map lỗi từ Identity sang mảng string để trả về
        var errors = result.Errors.Select(e => e.Description).ToArray();
        return Result<Guid>.Failure(errors);
    }
}