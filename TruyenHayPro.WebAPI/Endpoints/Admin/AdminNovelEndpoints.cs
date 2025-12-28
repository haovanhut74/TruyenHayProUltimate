using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Admin.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints.Admin;

public static class AdminNovelEndpoints
{
    public static void MapAdminNovelEndpoints(this IEndpointRouteBuilder app)
    {
        // Gom nhóm API Admin lại
        var group = app.MapGroup("/api/admin/novels")
            .RequireAuthorization(); // Bắt buộc đăng nhập (sau này thêm Role Admin)

        // GET: /api/admin/novels/all
        group.MapGet("/all", async (IAdminNovelService service) =>
        {
            var novels = await service.GetAllNovelsAsync();
            return Results.Ok(novels);
        });

        // POST: /api/admin/novels (Tạo mới)
        group.MapPost("/",
            async ([FromBody] CreateNovelDto dto, IAdminNovelService service, IValidator<CreateNovelDto> validator) =>
            {
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid) return Results.BadRequest(validationResult.ToDictionary());

                var id = await service.CreateNovelAsync(dto);
                return Results.Ok(id);
            });

        // PUT: /api/admin/novels (Cập nhật)
        group.MapPut("/", async (UpdateNovelDto dto, IAdminNovelService service) =>
        {
            await service.UpdateNovelAsync(dto);
            return Results.Ok();
        });

        // DELETE: /api/admin/novels/{id} (Xóa)
        group.MapDelete("/{id:guid}", async (Guid id, IAdminNovelService service) =>
        {
            await service.DeleteNovelAsync(id);
            return Results.Ok();
        });
    }
}