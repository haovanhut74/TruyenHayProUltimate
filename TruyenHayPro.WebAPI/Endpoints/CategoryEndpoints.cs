using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories");

        // 1. API Lấy danh sách thể loại (ĐÂY LÀ PHẦN CẦN THÊM)
        group.MapGet("/", async (ICategoryRepository repo) =>
        {
            var categories = await repo.GetAllAsync();
            // Chuyển đổi sang DTO
            var dtos = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return Results.Ok(dtos);
        });

        // 2. API Tạo thể loại (Giữ nguyên cái cũ)
        group.MapPost("/", CreateCategory);
    }

    static async Task<IResult> CreateCategory(CreateCategoryDto dto, [FromServices] ICategoryService service)
    {
        var id = await service.CreateCategoryAsync(dto);
        return Results.Ok(id);
    }
}