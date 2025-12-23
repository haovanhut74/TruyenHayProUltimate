using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Common.Interfaces.Repositories;

public interface IChapterRepository
{
    Task<Chapter> AddAsync(Chapter chapter);

    // Kiểm tra xem số chương đã tồn tại chưa (tránh trùng chương 1 hai lần)
    Task<bool> IsChapterNumberExistsAsync(Guid novelId, int chapterNumber);
    Task<Chapter?> GetChapterDetailAsync(Guid id);
    Task<Chapter?> GetNextChapterAsync(Guid novelId, int currentOrderIndex);
    Task<Chapter?> GetPreviousChapterAsync(Guid novelId, int currentOrderIndex);

    Task<Chapter?> GetChapterWithNovelAsync(Guid id);
    Task UpdateAsync(Chapter chapter);
}