using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        // Tạo nhóm đường dẫn riêng: /api/categories
        var group = app.MapGroup("/api/categories");

        // POST: /api/categories
        group.MapPost("/", CreateCategory);
    }

    static async Task<IResult> CreateCategory(CreateCategoryDto dto, ICategoryService service)
    {
        var id = await service.CreateCategoryAsync(dto);
        return Results.Ok(id);
    }
}