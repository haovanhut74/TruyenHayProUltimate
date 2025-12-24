using MediatR;
using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Application.Features.Chapters.Create;
using TruyenHayPro.Application.Features.Chapters.Delete;
using TruyenHayPro.Application.Features.Chapters.Queries.GetByNovel;
using TruyenHayPro.Application.Features.Chapters.Update;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class ChapterEndpoints
{
    public static void MapChapterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/chapters");

        group.MapPost("/", async (ISender sender, CreateChapterDto dto) =>
        {
            var result = await sender.Send(new CreateChapterCommand(dto));
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.BadRequest(result);
        });
        // Thêm vào MapChapterEndpoints
        group.MapGet("/{id:guid}", GetChapterDetail);
        group.MapPut("/", async (ISender sender, [FromBody] UpdateChapterDto dto) =>
        {
            var result = await sender.Send(new UpdateChapterCommand(dto));
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
        }).RequireAuthorization();
        
        group.MapGet("/novel/{novelId:guid}", async (Guid novelId, ISender mediator) =>
        {
            var result = await mediator.Send(new GetChaptersByNovelQuery(novelId));
            return Results.Ok(result);
        });
        
        // THÊM ENDPOINT DELETE:
        group.MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var result = await mediator.Send(new DeleteChapterCommand(id));

                return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
            })
            .RequireAuthorization(); // Bắt buộc đăng nhập
    }


    // Handler xử lý
    private static async Task<IResult> GetChapterDetail(
        Guid id,
        IChapterRepository chapterRepo)
    {
        var chapter = await chapterRepo.GetChapterDetailAsync(id);
        if (chapter == null) return Results.NotFound();

        var nextChap = await chapterRepo.GetNextChapterAsync(chapter.NovelId, chapter.OrderIndex);
        var prevChap = await chapterRepo.GetPreviousChapterAsync(chapter.NovelId, chapter.OrderIndex);

        // Map thủ công hoặc dùng AutoMapper (ở đây ta map tay cho nhanh)
        var dto = new ChapterDetailDto
        {
            Id = chapter.Id,
            NovelId = chapter.NovelId,
            NovelTitle = chapter.Novel.Title,
            Title = chapter.Title,
            Content = chapter.Content ?? "",
            OrderIndex = chapter.OrderIndex,
            CreatedDate = chapter.CreatedDate,
            NextChapterId = nextChap?.Id,
            PreviousChapterId = prevChap?.Id
        };

        return Results.Ok(dto);
    }
}