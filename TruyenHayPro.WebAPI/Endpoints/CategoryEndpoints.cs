using Microsoft.Extensions.Caching.Memory;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories");

        // GET: Lấy danh sách (Có Cache 30 phút)
        group.MapGet("/", async (ICategoryRepository repo, IMemoryCache cache) =>
        {
            // Nếu có trong Cache thì lấy ra, chưa có thì gọi DB rồi lưu vào Cache
            return await cache.GetOrCreateAsync("categories_list", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30); // Lưu 30 phút
                var categories = await repo.GetAllAsync();
                return categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
            });
        });

        group.MapPost("/", CreateCategory);
    }

    static async Task<IResult> CreateCategory(CreateCategoryDto dto, ICategoryService service)
    {
        var id = await service.CreateCategoryAsync(dto);
        return Results.Ok(id);
    }
}