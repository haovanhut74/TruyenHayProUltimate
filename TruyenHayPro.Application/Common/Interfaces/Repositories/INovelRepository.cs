using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Common.Interfaces.Repositories;

public interface INovelRepository
{
    // 1. Nhiệm vụ: Lấy danh sách truyện mới nhất (để hiện trang chủ)
    // Đầu vào: count (số lượng muốn lấy, ví dụ 10 cuốn)
    // Đầu ra: Danh sách Novel (Entity gốc)
    Task<List<Novel>> GetNovelsHomeAsync(int count);

    IQueryable<Novel> Query();

    // 2. Nhiệm vụ: Lấy chi tiết 1 cuốn truyện (kèm theo các chương và thể loại)
    // Đầu vào: id (mã truyện)
    // Đầu ra: Một cuốn Novel (hoặc null nếu không tìm thấy)
    Task<Novel?> GetNovelByIdAsync(Guid id);

    Task<Guid> AddAsync(Novel novel);
    Task UpdateAsync(Novel novel);

    Task DeleteAsync(Novel novel);
}