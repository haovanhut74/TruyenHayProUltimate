using System.Net.Http.Json;
using System.Text.Json; // Thêm thư viện này
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services;

public class ClientChapterService
{
    private readonly HttpClient _httpClient;

    // Cấu hình để parse JSON không phân biệt hoa thường
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public ClientChapterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<Guid>> CreateChapterAsync(CreateChapterDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("bff/chapters", request);
        var content = await response.Content.ReadAsStringAsync();

        // Dù thành công hay thất bại, hãy cố gắng đọc nó dưới dạng Result<Guid>
        try
        {
            var result = JsonSerializer.Deserialize<Result<Guid>>(content, _options);
            return result ?? Result<Guid>.Failure("Lỗi không xác định từ Server.");
        }
        catch
        {
            // Trường hợp xấu nhất: Server chết hoặc trả về HTML lỗi
            return Result<Guid>.Failure("Không thể kết nối đến hệ thống.");
        }
    }

    public async Task<ChapterDetailDto?> GetChapterDetailAsync(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ChapterDetailDto>($"bff/chapters/{id}");
        }
        catch
        {
            return null;
        }
    }
}