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
            var response = await api.PostAsJsonAsync("api/chapters", request);
            return await response.Content.ReadAsStringAsync();
        });
    }
}