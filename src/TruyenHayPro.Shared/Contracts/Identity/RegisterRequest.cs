using System.ComponentModel.DataAnnotations;

namespace TruyenHayPro.Shared.Contracts.Identity;

public class RegisterRequest
{
    [Required] public string FullName { get; set; } = string.Empty;

    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] public string Username { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Mật khẩu không khớp")]
    public string ConfirmPassword { get; set; } = string.Empty;
}