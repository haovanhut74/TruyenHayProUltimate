using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class NovelsEndpoints
{
    // Hàm này dùng để đăng ký tất cả các đường dẫn của Novels
    public static void MapNovelEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/novels");
        group.MapGet("home", GetNovelsHome);
        group.MapGet("{id:guid}", GetNovelById);

        // Cập nhật hàm POST
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

    // Hàm tạo mới có Validation
    static async Task<IResult> CreateNovel([FromBody] CreateNovelDto dto, INovelService service,
        IValidator<CreateNovelDto> validator)
    {
        // 1. Kiểm tra tính hợp lệ
        var validationResult = await validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.ToDictionary());
        }

        // 2. Nếu ổn thì mới tạo
        var id = await service.CreateNovelAsync(dto);
        return Results.Ok(id);
    }
}