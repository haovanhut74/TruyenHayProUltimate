namespace TruyenHayPro.Blazor.Endpoints;

public static class BffCategoryEndpoints
{
    public static void MapBffCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bff/categories");

        group.MapGet("", async (IHttpClientFactory factory) =>
        {
            var client = factory.CreateClient("WebAPI");
            var response = await client.GetAsync("/api/categories");

            if (!response.IsSuccessStatusCode)
                return Results.BadRequest();

            var data = await response.Content.ReadAsStringAsync();
            return Results.Content(data, "application/json");
        });
    }
}