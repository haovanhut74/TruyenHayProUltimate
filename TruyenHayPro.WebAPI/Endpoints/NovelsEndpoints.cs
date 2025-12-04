using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class NovelsEndpoints
{
    // Hàm này dùng để đăng ký tất cả các đường dẫn của Novels
    public static void MapNovelEndpoints(this IEndpointRouteBuilder app)
    {
        // Tạo một nhóm đường dẫn bắt đầu bằng /api/novels
        var group = app.MapGroup("/api/novels");

        // 1. API: Lấy danh sách trang chủ
        // GET /api/novels/home
        group.MapGet("home", GetNovelsHome);

        // 2. API: Lấy chi tiết truyện
        // GET /api/novels/{id}
        group.MapGet("{id:guid}", GetNovelById);

        // POST: /api/novels
        group.MapPost("/", CreateNovel);
    }
    // --- CÁC HÀM XỬ LÝ (HANDLERS) ---

    // Chú ý: Ta tiêm INovelService trực tiếp vào tham số hàm
    static async Task<IResult> GetNovelsHome(INovelService novelService)
    {
        // Lấy 10 truyện
        var novels = await novelService.GetNovelsHomeAsync(10);
        return Results.Ok(novels);
    }

    static async Task<IResult> GetNovelById(Guid id, INovelService novelService)
    {
        var novel = await novelService.GetNovelByIdAsync(id);

        return novel == null ? Results.NotFound() : Results.Ok(novel);
    }

    // --- HÀM XỬ LÝ ---
    static async Task<IResult> CreateNovel(CreateNovelDto dto, INovelService service)
    {
        var id = await service.CreateNovelAsync(dto);
        return Results.Ok(id);
    }
}