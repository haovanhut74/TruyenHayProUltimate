using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.DTO;
using System.Net.Http.Headers;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffChapterEndpoints
{
    public static void MapBffChapterEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. KHÔNG dùng .RequireAuthorization() -> Tránh bị Redirect 302/Lỗi 405
        var group = app.MapGroup("/bff/chapters");

        group.MapPost("",
            async ([FromBody] CreateChapterDto request, IHttpClientFactory factory, HttpContext context) =>
            {
                // 2. BẢO MẬT THỦ CÔNG: Kiểm tra Cookie "authToken"
                // Đây là chốt chặn đầu tiên. Nếu hacker không có cookie -> Chặn luôn.
                var token = context.Request.Cookies["authToken"];

                if (string.IsNullOrWhiteSpace(token))
                {
                    // Trả về 401 Unauthorized chuẩn xác
                    // Client nhận được mã này sẽ biết đường tự xử lý (hiện popup login, v.v.)
                    return Results.Unauthorized();
                }

                // 3. Gọi Backend (WebAPI) với Token
                var client = factory.CreateClient("WebAPI");

                // Gắn Token vào Header để Backend xác thực lần 2 (Lớp bảo mật tuyệt đối)
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync("api/chapters", request);

                // Xử lý kết quả từ Backend trả về
                if (!response.IsSuccessStatusCode)
                {
                    // Nếu Backend trả 401/403 -> Trả về y hệt cho Client
                    return Results.StatusCode((int)response.StatusCode);
                }

                var json = await response.Content.ReadAsStringAsync();
                return Results.Content(json, "application/json");
            });
    }
}