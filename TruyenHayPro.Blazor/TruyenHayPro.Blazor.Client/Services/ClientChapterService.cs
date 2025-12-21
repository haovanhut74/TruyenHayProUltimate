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
        var response = await _httpClient. PostAsJsonAsync("bff/chapters", request);

        if (!response.IsSuccessStatusCode)
        {
            var raw = await response.Content.ReadAsStringAsync();
            return Result<Guid>.Failure(raw);
        }

        return await response.Content.ReadFromJsonAsync<Result<Guid>>();

    }
}