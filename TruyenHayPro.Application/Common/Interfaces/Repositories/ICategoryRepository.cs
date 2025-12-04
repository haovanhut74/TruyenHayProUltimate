using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Common.Interfaces.Repositories;

public interface ICategoryRepository
{
    // Tạo mới
    Task<Guid> AddAsync(Category category);

    // Lấy danh sách (để sau này hiện lên combobox chọn)
    Task<List<Category>> GetAllAsync();
}