using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Application.Common.Interfaces.Services;

public interface INovelService
{
    // Đầu ra bây giờ là DTO (Cái Menu), không phải Entity gốc nữa
    Task<List<NovelDto>> GetNovelsHomeAsync(int count);

    Task<NovelDto?> GetNovelByIdAsync(Guid id);
}