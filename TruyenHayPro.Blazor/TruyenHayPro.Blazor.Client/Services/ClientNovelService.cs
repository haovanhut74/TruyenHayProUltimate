using System.Net.Http.Json;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Services;

public class ClientNovelService : INovelService
{
    private readonly HttpClient _http;

    public ClientNovelService(HttpClient http)
    {
        _http = http;
    }

    // --- CÁC HÀM LẤY DỮ LIỆU ---
    public async Task<List<NovelDto>> GetNovelsHomeAsync(int count)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<NovelDto>>($"api/novels/home?count={count}");
            return result ?? new List<NovelDto>();
        }
        catch
        {
            return new List<NovelDto>();
        }
    }

    public async Task<NovelDto?> GetNovelByIdAsync(Guid id)
    {
        try
        {
            return await _http.GetFromJsonAsync<NovelDto>($"api/novels/{id}");
        }
        catch
        {
            return null;
        }
    }

    // --- CÁC HÀM TẠO MỚI (Bắt buộc phải có vì Interface yêu cầu) ---
    // Ở trang chủ ta chưa dùng tới chức năng thêm, nên tạm thời cứ để nó ném lỗi hoặc trả về 0
    // Sau này làm trang Admin bên Client thì ta sẽ viết code gọi API POST ở đây.

    public Task<Guid> CreateNovelAsync(CreateNovelDto dto)
    {
        throw new NotImplementedException("Client chưa hỗ trợ tạo truyện lúc này.");
    }

    public Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
    {
        throw new NotImplementedException();
    }

    // Nếu phu quân đã lỡ thêm hàm CreateTagAsync vào Interface thì thêm dòng này:
    public Task<Guid> CreateTagAsync(CreateTagDto dto) => throw new NotImplementedException();
}