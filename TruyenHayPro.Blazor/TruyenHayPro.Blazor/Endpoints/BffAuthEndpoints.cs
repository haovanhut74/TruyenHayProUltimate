using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffAuthEndpoints
{
    public static void MapBffEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/auth");

        // 1. API Login: Nhận User/Pass -> Gọi WebAPI -> Lưu Cookie
        group.MapPost("/login",
            async (LoginRequest request, IHttpClientFactory httpClientFactory, HttpContext context) =>
            {
                var client = httpClientFactory.CreateClient("WebAPI");
                var response = await client.PostAsJsonAsync("/api/auth/login", request);

                // ❌ Nếu WebAPI trả lỗi → KHÔNG parse JSON
                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();

                    return Results.BadRequest(
                        Result<AuthResponse>.Failure(errorText)
                    );
                }

                // ✅ Chỉ parse JSON khi chắc chắn 200
                var result = await response.Content.ReadFromJsonAsync<Result<AuthResponse>>();
                var token = result!.Value.Token;

                context.Response.Cookies.Append("authToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // <--- Bắt buộc phải có khi deploy Production
                    SameSite = SameSiteMode.Strict, // <--- Chống CSRF rất tốt
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return Results.Ok(Result<AuthResponse>.Success(
                    result.Value with { Token = "" }));
            });


        // 2. API Logout: Xóa Cookie
        group.MapPost("/logout", (HttpContext context) =>
        {
            context.Response.Cookies.Delete("authToken");
            return Results.Ok();
        });

        group.MapGet("/user-info", async (IHttpClientFactory factory, HttpContext context) =>
        {
            // 1. Lấy token từ Cookie
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token))
                return Results.Unauthorized();

            // 2. Tạo client gọi WebAPI
            var client = factory.CreateClient("WebAPI");

            // 🔥 QUAN TRỌNG: Đẩy token sang WebAPI
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // 3. Gọi API /me
            var response = await client.GetAsync("/api/auth/me");

            if (!response.IsSuccessStatusCode)
                return Results.Unauthorized();

            // 4. Trả thẳng JSON user về cho Client
            var userInfo = await response.Content.ReadFromJsonAsync<UserInfoDto>();
            return Results.Ok(userInfo);
        });
    }
}