using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Admin.Interfaces.Services;

namespace TruyenHayPro.Admin.Endpoints;

public static class AdminBffNovelEndpoints
{
    public static void MapAdminBffNovelEndpoints(this IEndpointRouteBuilder app)
    {
        // Đây là đường dẫn mà Admin Client sẽ gọi lên (trùng khớp với ClientNovelService)
        var group = app.MapGroup("/bff/novels");

        // API: Lấy tất cả truyện
        group.MapGet("/all", async (IAdminNovelService service) =>
        {
            // Admin Server gọi tiếp sang WebAPI hoặc xử lý logic tại đây
            var result = await service.GetAllNovelsAsync();
            return Results.Ok(result);
        }).RequireAuthorization(); // Bắt buộc admin phải đăng nhập

        // API: Xóa truyện
        group.MapDelete("/{id:guid}", async (Guid id, IAdminNovelService service) =>
        {
            await service.DeleteNovelAsync(id);
            return Results.Ok();
        }).RequireAuthorization();
    }
}