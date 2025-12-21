using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TruyenHayPro.Blazor.Client.Services.Interface;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Blazor.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly JsonSerializerOptions _options;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
            var response = await _httpClient.PostAsJsonAsync("/bff/auth/login", request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Cố gắng đọc lỗi từ Backend gửi về
                try
                {
                    return JsonSerializer.Deserialize<Result<AuthResponse>>(content, _options)!;
                }
                catch
                {
                    return Result<AuthResponse>.Failure("Lỗi hệ thống hoặc dữ liệu không hợp lệ.");
                }
            }

            return JsonSerializer.Deserialize<Result<AuthResponse>>(content, _options)!;
        }
        catch (Exception ex)
        {
            return Result<AuthResponse>.Failure($"Lỗi kết nối: {ex.Message}");
        }
    }

    // Triển khai hàm Logout
    public async Task LogoutAsync()
    {
        // 1. Gọi BFF để xóa HttpOnly Cookie (Lệnh này chỉ có tác dụng thực sự khi chạy ở Client/Browser)
        try 
        {
            await _httpClient.PostAsync("/bff/auth/logout", null);
        }
        catch 
        {
            // Bỏ qua lỗi kết nối nếu có, để đảm bảo code bên dưới vẫn chạy
        }
        
        if (_authStateProvider is CustomAuthStateProvider customProvider)
        {
            customProvider.NotifyUserLoggedOut();
        }
    }
}