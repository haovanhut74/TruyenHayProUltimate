using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Application.Common.Admin.Interfaces.Services;

public interface IAdminNovelService
{
    Task<List<NovelDto>> GetAllNovelsAsync();
    Task<NovelDto?> GetNovelByIdAsync(Guid id);
    Task<Guid> CreateNovelAsync(CreateNovelDto dto);
    Task UpdateNovelAsync(UpdateNovelDto dto);
    Task DeleteNovelAsync(Guid id);
}