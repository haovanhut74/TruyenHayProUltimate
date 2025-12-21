using System.Net.Http.Json;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services;

public class ClientChapterService
{
    private readonly HttpClient _httpClient;

    public ClientChapterService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<Guid>> CreateChapterAsync(CreateChapterDto request)
    {
        // Gọi endpoint BFF (/bff/chapters) chứ không gọi trực tiếp API
        var response = await _httpClient.PostAsJsonAsync("/bff/chapters", request);

        // Đọc kết quả trả về
        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();
        return result!;
    }
}