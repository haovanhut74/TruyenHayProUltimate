using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Admin.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints.Admin;

public static class AdminNovelEndpoints
{
    public static void MapAdminNovelEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin/novels")
            .RequireAuthorization();

        // 1. GET: Thêm [FromServices]
        group.MapGet("/all", async ([FromServices] IAdminNovelService service) =>
        {
            var novels = await service.GetAllNovelsAsync();
            return Results.Ok(novels);
        });

        // 2. POST: Thêm [FromServices] cho cả service và validator
        group.MapPost("/",
            async (
                [FromBody] CreateNovelDto dto,
                [FromServices] IAdminNovelService service,
                [FromServices] IValidator<CreateNovelDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid) return Results.BadRequest(validationResult.ToDictionary());

                var id = await service.CreateNovelAsync(dto);
                return Results.Ok(id);
            });

        // 3. PUT: Thêm [FromServices]
        group.MapPut("/", async (UpdateNovelDto dto, [FromServices] IAdminNovelService service) =>
        {
            await service.UpdateNovelAsync(dto);
            return Results.Ok();
        });

        // 4. DELETE: Thêm [FromServices]
        group.MapDelete("/{id:guid}", async (Guid id, [FromServices] IAdminNovelService service) =>
        {
            await service.DeleteNovelAsync(id);
            return Results.Ok();
        });
    }
}