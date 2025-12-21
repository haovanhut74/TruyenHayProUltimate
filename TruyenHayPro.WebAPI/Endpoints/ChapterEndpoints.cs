using MediatR;
using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Application.Features.Chapters.Create;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class ChapterEndpoints
{
    public static void MapChapterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/chapters").WithTags("Chapters");

        group.MapPost("/", CreateChapter);
    }

    private static async Task<IResult> CreateChapter(ISender sender, [FromBody] CreateChapterDto request)
    {
        var command = new CreateChapterCommand(request);
        var result = await sender.Send(command);
        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }
}