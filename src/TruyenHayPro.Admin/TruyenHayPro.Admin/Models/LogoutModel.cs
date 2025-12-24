using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Admin.Models;

public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    // Chạy hàm này khi người dùng truy cập /logout
    public async Task<IActionResult> OnGet()
    {
        // Xóa Cookie đăng nhập
        await _signInManager.SignOutAsync();
        
        // Đá về trang đăng nhập
        return RedirectToPage("/Login");
    }
    
    // Hỗ trợ cả method POST nếu gọi từ form
    public async Task<IActionResult> OnPost()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Login");
    }
}