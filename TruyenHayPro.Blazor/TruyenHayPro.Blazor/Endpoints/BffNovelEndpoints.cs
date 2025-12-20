using System.Net.Http.Headers;
using System.Text;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffNovelEndpoints
{
    public static void MapBffNovelEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/novels");

        // 📌 Trang chủ
        group.MapGet("/home", async (int count, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");

            var response = await client.GetAsync($"/api/novels/home?count={count}");
            if (!response.IsSuccessStatusCode)
                return Results.StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });

        // 📌 Chi tiết truyện
        group.MapGet("/{id:guid}", async (Guid id, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");

            var response = await client.GetAsync($"/api/novels/{id}");
            if (!response.IsSuccessStatusCode)
                return Results.StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });

        // ✍️ Tạo truyện
        group.MapPost("", async (HttpContext context, IHttpClientFactory factory) =>
        {
            // 1. Lấy JWT từ cookie
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token))
                return Results.Unauthorized();

            // 2. Tạo client
            var client = factory.CreateClient("WebAPI");

            // 🔥 ĐÍNH TOKEN
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 3. Đọc body
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            // 4. Gọi WebAPI
            var response = await client.PostAsync("/api/novels", content);

            if (!response.IsSuccessStatusCode)
                return Results.StatusCode((int)response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });


        // 🔐 GET: Truyện của tôi
        group.MapGet("/my", async (IHttpClientFactory factory, HttpContext context) =>
        {
            // 1. Lấy JWT từ Cookie
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token))
                return Results.Unauthorized();

            // 2. Tạo HttpClient gọi WebAPI
            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 3. Gọi WebAPI
            var response = await client.GetAsync("/api/novels/my");

            if (!response.IsSuccessStatusCode)
                return Results.StatusCode((int)response.StatusCode);

            // 4. Trả JSON nguyên vẹn
            var json = await response.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        });
        // ✏️ Sửa truyện
        group.MapPut("", async (HttpContext context, IHttpClientFactory factory) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token))
                return Results.Unauthorized();

            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("/api/novels", content);

            return Results.StatusCode((int)response.StatusCode);
        });

        // 🗑️ Xóa truyện
        group.MapDelete("/{id:guid}", async (Guid id, IHttpClientFactory factory, HttpContext context) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token))
                return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"/api/novels/{id}");

            return !response.IsSuccessStatusCode ? Results.StatusCode((int)response.StatusCode) : Results.Ok();
        });
    }
}