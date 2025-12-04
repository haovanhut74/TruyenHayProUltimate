using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Common.Interfaces.Repositories;

public interface ITagRepository
{
    Task<Guid> AddAsync(Tag tag);
    Task<List<Tag>> GetAllAsync();
    // Lấy danh sách Tag dựa theo một list ID gửi vào
    Task<List<Tag>> GetListByIdsAsync(List<Guid> ids);
}