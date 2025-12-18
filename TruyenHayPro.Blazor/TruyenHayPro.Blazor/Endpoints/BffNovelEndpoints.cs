using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Endpoints;

public static class BffNovelEndpoints
{
    public static void MapBffNovelEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/novels");

        group.MapGet("/home", async (int count, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");

            var response = await client.GetAsync($"/api/novels/home?count={count}");
            if (!response.IsSuccessStatusCode)
                return Results.StatusCode((int)response.StatusCode);

            var data = await response.Content.ReadFromJsonAsync<List<NovelDto>>();
            return Results.Ok(data);
        });


        group.MapGet("/{id:guid}", async (Guid id, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");

            var response = await client.GetAsync($"/api/novels/{id}");
            if (!response.IsSuccessStatusCode)
                return Results.StatusCode((int)response.StatusCode);

            var data = await response.Content.ReadFromJsonAsync<NovelDto>();
            return Results.Ok(data);
        });


        group.MapPost("", async (HttpRequest req, IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");

            var content = new StreamContent(req.Body);
            content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync("/api/novels", content);

            if (!response.IsSuccessStatusCode)
                return Results.BadRequest(await response.Content.ReadAsStringAsync());

            var id = await response.Content.ReadFromJsonAsync<Guid>();
            return Results.Ok(id);
        });

    }
}