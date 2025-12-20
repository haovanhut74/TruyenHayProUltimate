using System.Net.Http.Json;
using System.Text.Json;
using TruyenHayPro.Blazor.Client.Services.Interface;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<Result<Guid>> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();
        return result!;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/bff/auth/login", request);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            try
            {
                return JsonSerializer.Deserialize<Result<AuthResponse>>(content, _options)!;
            }
            catch
            {
                return Result<AuthResponse>.Failure("Đăng nhập thất bại.");
            }
        }

        return JsonSerializer.Deserialize<Result<AuthResponse>>(content, _options)!;
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("/bff/auth/logout", null);
    }
}