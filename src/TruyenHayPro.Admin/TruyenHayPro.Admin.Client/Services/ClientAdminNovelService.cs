using System.Net.Http.Json;
using TruyenHayPro.Application.Common.Admin.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Admin.Client.Services;

public class ClientAdminNovelService : IAdminNovelService
{
    private readonly HttpClient _http;

    public ClientAdminNovelService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<NovelDto>> GetAllNovelsAsync()
    {
        // QUAN TRỌNG: Phải gọi đúng đường dẫn "bff" mà bạn đã map bên server
        return await _http.GetFromJsonAsync<List<NovelDto>>("bff/novels/all") ?? [];
    }

    async Task<NovelDto?> IAdminNovelService.GetNovelByIdAsync(Guid id)
    {
        return await GetNovelByIdAsync(id);
    }

    async Task<Guid> IAdminNovelService.CreateNovelAsync(CreateNovelDto dto)
    {
        return await CreateNovelAsync(dto);
    }

    async Task IAdminNovelService.UpdateNovelAsync(UpdateNovelDto dto)
    {
        await UpdateNovelAsync(dto);
    }

    // Các hàm khác (nếu cần) thì cũng gọi "bff/..." tương ứng
    // Ví dụ Delete:
    public async Task DeleteNovelAsync(Guid id)
    {
        await _http.DeleteAsync($"bff/novels/{id}");
    }

    // Các hàm chưa dùng tới có thể throw NotImplemented hoặc để trống tạm thời
    async Task<List<NovelDto>> IAdminNovelService.GetAllNovelsAsync()
    {
        return await GetAllNovelsAsync();
    }

    public Task<NovelDto?> GetNovelByIdAsync(Guid id) => throw new NotImplementedException();
    public Task<Guid> CreateNovelAsync(CreateNovelDto dto) => throw new NotImplementedException();
    public Task UpdateNovelAsync(UpdateNovelDto dto) => throw new NotImplementedException();
}