using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffChapterEndpoints
{
    public static void MapBffChapterEndpoints(this IEndpointRouteBuilder app)
    {
        // Endpoint này yêu cầu phải đăng nhập
        var group = app.MapGroup("/bff/chapters").RequireAuthorization();

        group.MapPost("/", async ([FromBody] CreateChapterDto request, HttpClient api) =>
        {
            // Proxy: Nhận từ Client -> Gửi sang WebAPI
            var response = await api.PostAsJsonAsync("api/chapters", request);

            // Trả nguyên kết quả từ WebAPI về cho Client
            return await response.Content.ReadAsStringAsync(); 
            // Hoặc tối ưu hơn: return Results.StatusCode((int)response.StatusCode);
        });
    }
}