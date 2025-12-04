using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class TagEndpoints
{
    public static void MapTagEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tags");

        // POST: /api/tags
        group.MapPost("/", CreateTag);
    }

    static async Task<IResult> CreateTag(CreateTagDto dto, ITagService service)
    {
        var id = await service.CreateTagAsync(dto);
        return Results.Ok(id);
    }
}