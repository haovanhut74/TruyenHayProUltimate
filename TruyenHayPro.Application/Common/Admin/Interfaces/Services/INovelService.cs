using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Application.Common.Admin.Interfaces.Services;

public interface INovelService
{
    Task<List<NovelDto>> GetAllNovelsAsync();
}