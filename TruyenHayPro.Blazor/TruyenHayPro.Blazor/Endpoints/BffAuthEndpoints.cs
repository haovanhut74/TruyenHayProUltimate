using Microsoft.AspNetCore.Identity;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Shared.Contracts.Identity;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffAuthEndpoints
{
    public static void MapBffEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/auth");

        // 1. API Login: Nhận User/Pass -> Gọi WebAPI -> Lưu Cookie
        group.MapPost("/login",
            async (
                LoginRequest request,
                SignInManager<ApplicationUser> signInManager
            ) =>
            {
                var result = await signInManager.PasswordSignInAsync(
                    request.Username,
                    request.Password,
                    isPersistent: true,
                    lockoutOnFailure: false
                );

                return !result.Succeeded ? Results.Unauthorized() : Results.Ok();
            });


        // 2. API Logout: Xóa Cookie
        group.MapPost("/logout",
            async (SignInManager<ApplicationUser> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            });


        group.MapGet("/me", (HttpContext context) =>
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return Results.Unauthorized();

            return Results.Ok(new
            {
                Username = context.User.Identity.Name
            });
        });
    }
}