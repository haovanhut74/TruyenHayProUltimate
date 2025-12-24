using System.ComponentModel.DataAnnotations;

namespace TruyenHayPro.Admin.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}