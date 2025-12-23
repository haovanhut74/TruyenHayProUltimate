using System.Net.Http.Json;
using System.Text.Json;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services;

public class ClientChapterService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public ClientChapterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // 1. Giữ nguyên hàm Create
    public async Task<Result<Guid>> CreateChapterAsync(CreateChapterDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("bff/chapters", request);
        return await DeserializeResultAsync<Guid>(response);
    }

    // 2. THÊM MỚI hàm Update (để sửa lỗi ở EditChapter)
    public async Task<Result<Guid>> UpdateChapterAsync(UpdateChapterDto request)
    {
        var response = await _httpClient.PutAsJsonAsync("bff/chapters", request);
        return await DeserializeResultAsync<Guid>(response);
    }

    // 3. SỬA LẠI hàm GetDetail (Trả về Result<T> thay vì Dto?)
    public async Task<Result<ChapterDetailDto>> GetChapterDetailAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"bff/chapters/{id}");

            if (response.IsSuccessStatusCode)
            {
                var dto = await response.Content.ReadFromJsonAsync<ChapterDetailDto>();
                return dto != null
                    ? Result<ChapterDetailDto>.Success(dto)
                    : Result<ChapterDetailDto>.Failure("Dữ liệu trả về rỗng.");
            }

            return Result<ChapterDetailDto>.Failure("Không tìm thấy chương.");
        }
        catch
        {
            return Result<ChapterDetailDto>.Failure("Lỗi kết nối đến server.");
        }
    }

    // Hàm phụ trợ để tái sử dụng việc đọc JSON Result
    private async Task<Result<T>> DeserializeResultAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            var result = JsonSerializer.Deserialize<Result<T>>(content, _options);
            return result ?? Result<T>.Failure("Lỗi không xác định.");
        }
        catch
        {
            return Result<T>.Failure($"Lỗi kết nối ({response.StatusCode}).");
        }
    }

    // 4. THÊM hàm lấy danh sách chương (cho trang ManageChapters)
    public async Task<Result<List<ChapterDto>>> GetChaptersByNovelIdAsync(Guid novelId)
    {
        // Gọi endpoint BFF (lưu ý đường dẫn phải khớp với BffChapterEndpoints)
        var response = await _httpClient.GetAsync($"bff/chapters/novel/{novelId}");

        // Tái sử dụng hàm DeserializeResultAsync đã có
        return await DeserializeResultAsync<List<ChapterDto>>(response);
    }

    // 5. SỬA LẠI hàm DeleteChapterAsync (đang bị lỗi)
    // Đổi IResult<Guid> thành Result<Guid>
    public async Task<Result<Guid>> DeleteChapterAsync(Guid chapterId)
    {
        var response = await _httpClient.DeleteAsync($"bff/chapters/{chapterId}");

        // Dùng DeserializeResultAsync thay vì ToResult
        return await DeserializeResultAsync<Guid>(response);
    }
}