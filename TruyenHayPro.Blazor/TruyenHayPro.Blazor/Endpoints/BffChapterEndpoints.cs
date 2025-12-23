using System.Net.Http.Headers;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffChapterEndpoints
{
    public static void MapBffChapterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/chapters");

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

            // ⭐ LUÔN TRẢ BODY
            return Results.Content(
                json,
                "application/json",
                statusCode: (int)response.StatusCode
            );
        });

        // Thêm vào group
        group.MapGet("/{id:guid}", async (Guid id, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");
            var response = await client.GetAsync($"/api/chapters/{id}");

            if (!response.IsSuccessStatusCode) return Results.StatusCode((int)response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            return Results.Content(content, "application/json");
        });
    }
}