using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using TruyenHayPro.Blazor.Client.Utils;

namespace TruyenHayPro.Blazor.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    // Biến này để lưu "Thần Hồn" trong RAM, tránh đọc ổ cứng liên tục
    private ClaimsPrincipal? _cachedUser;

    public CustomAuthStateProvider(ILocalStorageService localStorage, HttpClient http)
    {
        _localStorage = localStorage;
        _http = http;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // 1. Nếu đã có trong RAM thì trả về ngay (Tốc độ ánh sáng)
        if (_cachedUser != null)
        {
            return new AuthenticationState(_cachedUser);
        }

        // 2. Nếu chưa có, mới đi đọc LocalStorage (Tốn thời gian)
        var token = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(token))
        {
            // Không có token -> Khách vãng lai
            _cachedUser = new ClaimsPrincipal(new ClaimsIdentity());
        }
        else
        {
            // Có token -> Thiết lập danh tính
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var claims = JwtParser.ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _cachedUser = new ClaimsPrincipal(identity);
        }

        return new AuthenticationState(_cachedUser);
    }

    public void NotifyUserLoggedIn(string token)
    {
        var claims = JwtParser.ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        _cachedUser = new ClaimsPrincipal(identity); // Lưu ngay vào RAM

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var authState = Task.FromResult(new AuthenticationState(_cachedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLoggedOut()
    {
        _cachedUser = new ClaimsPrincipal(new ClaimsIdentity()); // Xóa khỏi RAM
        _http.DefaultRequestHeaders.Authorization = null;

        var authState = Task.FromResult(new AuthenticationState(_cachedUser));
        NotifyAuthenticationStateChanged(authState);
    }
}