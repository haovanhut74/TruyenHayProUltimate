using System.Net.Http.Headers;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffChapterEndpoints
{
    public static void MapBffChapterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/chapters");

        // 1. Create
        group.MapPost("", async (
            CreateChapterDto request,
            IHttpClientFactory factory,
            HttpContext context) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token))
                return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/chapters", request);
            var json = await response.Content.ReadAsStringAsync();

            return Results.Content(json, "application/json", statusCode: (int)response.StatusCode);
        });

        // 2. Get Detail (Lấy 1 chương)
        group.MapGet("/{id:guid}", async (Guid id, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");
            var response = await client.GetAsync($"/api/chapters/{id}");

            var content = await response.Content.ReadAsStringAsync();
            return Results.Content(content, "application/json", statusCode: (int)response.StatusCode);
        });

        // =======================================================
        // 👇👇👇 THÊM ĐOẠN NÀY ĐỂ FIX LỖI 404 👇👇👇
        // 3. Get List by Novel (Lấy danh sách chương theo truyện)
        group.MapGet("/novel/{novelId:guid}", async (Guid novelId, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");
            // Gọi sang API Backend (phải đảm bảo bên WebAPI có endpoint này)
            var response = await client.GetAsync($"/api/chapters/novel/{novelId}");

            var content = await response.Content.ReadAsStringAsync();
            return Results.Content(content, "application/json", statusCode: (int)response.StatusCode);
        });
        // =======================================================

        // 4. Update
        group.MapPut("", async (UpdateChapterDto request, IHttpClientFactory factory, HttpContext context) =>
        {
            var token = context.Request.Cookies["authToken"];
            if (string.IsNullOrWhiteSpace(token)) return Results.Unauthorized();

            var client = factory.CreateClient("WebAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync("api/chapters", request);

            var content = await response.Content.ReadAsStringAsync();
            return Results.Content(content, "application/json", statusCode: (int)response.StatusCode);
        });

        // 5. Delete
        group.MapDelete("/{id:guid}", async (Guid id, IHttpClientFactory httpClientFactory, HttpContext context) =>
            {
                // Lấy token để truyền sang API (phòng trường hợp API yêu cầu quyền)
                var token = context.Request.Cookies["authToken"];
                var client = httpClientFactory.CreateClient("WebAPI");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await client.DeleteAsync($"/api/chapters/{id}");

                var content = await response.Content.ReadAsStringAsync();
                return Results.Content(content, "application/json", statusCode: (int)response.StatusCode);
            })
            .RequireAuthorization();
    }
}