using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.DTO;
using TruyenHayPro.Shared.Wrapper;

// Trỏ về User entity của anh

namespace TruyenHayPro.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
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

    public async Task<Result<AuthResponse>> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return Result<AuthResponse>.Failure("Tài khoản không tồn tại.");

        var isValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isValid) return Result<AuthResponse>.Failure("Mật khẩu không chính xác.");

        var token = GenerateJwtToken(user);
        return Result<AuthResponse>.Success(new AuthResponse(user.Id, user.UserName!, user.Email!, token));
    }

    public Guid GetUserId(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
    }

    public async Task<UserInfoDto> GetUserInfoAsync(Guid userId)
    {
        var user = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto
            {
                Id = u.Id,
                Username = u.UserName!,
                FullName = u.FullName,
                Email = u.Email
            })
            .FirstOrDefaultAsync();

        return user!;
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new("FullName", user.FullName ?? user.UserName!)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}