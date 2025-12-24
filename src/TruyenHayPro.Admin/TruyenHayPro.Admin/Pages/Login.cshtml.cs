using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TruyenHayPro.Admin.Models;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Admin.Pages;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet(string? returnUrl = null)
    {
        // Nếu đã login rồi thì đá về trang chủ
        if (User.Identity?.IsAuthenticated == true)
        {
            Response.Redirect("/");
        }
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= "/";

        if (!ModelState.IsValid) return Page();

        // 1. Tìm user
        var user = await _userManager.FindByNameAsync(Input.Username);
        if (user == null)
        {
            ErrorMessage = "Tài khoản không tồn tại.";
            return Page();
        }

        // 2. (BẢO MẬT) Kiểm tra xem có phải là Admin không?
        // Tạm thời mình comment lại để bạn test đăng nhập trước đã. 
        // Sau này uncomment dòng dưới để chặn user thường.
        /*
        if (!await _userManager.IsInRoleAsync(user, "Admin") && !await _userManager.IsInRoleAsync(user, "SuperAdmin"))
        {
            ErrorMessage = "Bạn không có quyền truy cập trang quản trị.";
            return Page();
        }
        */

        // 3. Thực hiện đăng nhập bằng Cookie
        var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            ErrorMessage = "Sai mật khẩu.";
            return Page();
        }
    }
}