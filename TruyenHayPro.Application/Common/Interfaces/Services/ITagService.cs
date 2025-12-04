using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Application.Common.Interfaces.Services;

public interface ITagService
{
    Task<Guid> CreateTagAsync(CreateTagDto dto);
}