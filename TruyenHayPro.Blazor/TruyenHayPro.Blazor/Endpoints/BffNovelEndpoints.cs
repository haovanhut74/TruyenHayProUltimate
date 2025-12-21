using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc; // Cần thêm cái này để dùng [FromBody]
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffNovelEndpoints
{
    public static void MapBffNovelEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/novels");

        // 📌 Trang chủ (Public - Không cần Token)
        group.MapGet("/home", async (int count, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");
            // Public thì không cần gắn Header Authorization
            var response = await client.GetAsync($"/api/novels/home?count={count}");
            if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });

        // 📌 Chi tiết truyện (Public - Không cần Token)
        group.MapGet("/{id:guid}", async (Guid id, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");
            var response = await client.GetAsync($"/api/novels/{id}");
            if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });

        // ✍️ Tạo truyện (Cần Token)
        // SỬA: Dùng [FromBody] CreateNovelDto thay vì đọc Stream
        group.MapPost("", async ([FromBody] CreateNovelDto request, HttpContext context, IHttpClientFactory factory) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token)) return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Gửi Object sang Backend (An toàn hơn gửi raw string)
            var response = await client.PostAsJsonAsync("/api/novels", request);

            if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });

        // 🔐 GET: Truyện của tôi (Cần Token)
        group.MapGet("/my", async (IHttpClientFactory factory, HttpContext context) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token)) return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/novels/my");
            if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });

        // ✏️ Sửa truyện (Cần Token + Validation)
        // SỬA: Dùng [FromBody] UpdateNovelDto
        group.MapPut("", async ([FromBody] UpdateNovelDto request, HttpContext context, IHttpClientFactory factory) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token)) return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync("/api/novels", request);
            return Results.StatusCode((int)response.StatusCode);
        });

        // 🗑️ Xóa truyện (Cần Token)
        group.MapDelete("/{id:guid}", async (Guid id, IHttpClientFactory factory, HttpContext context) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token)) return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/novels/{id}");
            return !response.IsSuccessStatusCode ? Results.StatusCode((int)response.StatusCode) : Results.Ok();
        });
    }
}