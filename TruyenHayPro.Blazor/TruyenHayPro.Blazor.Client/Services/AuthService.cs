using System.Net.Http.Json;
using System.Text.Json;
using TruyenHayPro.Blazor.Client.Services.Interface;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<Guid>> RegisterAsync(RegisterRequest request)
    {
        // Gọi đến API Backend. Lưu ý đường dẫn "/api/auth/register" phải khớp với bên Controller/Endpoint
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);


        // Đọc kết quả trả về từ API
        var result = await response.Content.ReadFromJsonAsync<Result<Guid>>();

        return result!;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!response.IsSuccessStatusCode)
            {
                // Cố gắng đọc lỗi từ Backend gửi về
                try
                {
                    return JsonSerializer.Deserialize<Result<AuthResponse>>(content, options)!;
                }
                catch
                {
                    return Result<AuthResponse>.Failure("Lỗi hệ thống hoặc dữ liệu không hợp lệ.");
                }
            }

            return JsonSerializer.Deserialize<Result<AuthResponse>>(content, options)!;
        }
        catch (Exception ex)
        {
            return Result<AuthResponse>.Failure($"Lỗi kết nối: {ex.Message}");
        }
    }
}