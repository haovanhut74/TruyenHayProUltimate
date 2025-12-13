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
    public async Task<Guid> CreateNovelAsync(CreateNovelDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/novels", dto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        // Đọc lỗi từ Server gửi về (nếu có)
        var errorContent = await response.Content.ReadAsStringAsync();
        throw new Exception($"Lỗi tạo truyện: {errorContent}");
    }

    public async Task<List<NovelDto>> GetNovelsHomeAsync(int count)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<NovelDto>>($"api/novels/home?count={count}") ?? [];
        }
        catch
        {
            return [];
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


    // --- THÊM HÀM NÀY ĐỂ LẤY THỂ LOẠI ---
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        try
        {
            // Gọi đến API vừa tạo ở Bước 2
            return await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories") ?? [];
        }
        catch
        {
            // Nếu lỗi thì trả về danh sách rỗng để không crash app
            return new();
        }
    }
}