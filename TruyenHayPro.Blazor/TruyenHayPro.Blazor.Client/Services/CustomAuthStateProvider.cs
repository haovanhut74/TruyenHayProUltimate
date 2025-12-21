using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TruyenHayPro.Shared.DTO;

namespace TruyenHayPro.Blazor.Client.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _http;
    private readonly PersistentComponentState _state; // Inject thêm cái này
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public CustomAuthStateProvider(HttpClient http, PersistentComponentState state)
    {
        _http = http;
        _state = state;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // 1. ƯU TIÊN: Kiểm tra xem Server có gửi dữ liệu xuống không ("Persisted State")
        if (_state.TryTakeFromJson<UserInfoDto>("UserInfo", out var userInfo))
        {
            // Nếu có, dùng luôn -> KHÔNG CẦN GỌI API -> Nhanh tức thì
            return CreateAuthenticationState(userInfo);
        }

        // 2. Nếu không có (ví dụ user F5 xong rồi điều hướng sang trang khác), mới gọi API BFF
        try
        {
            var response = await _http.GetAsync("/bff/auth/user-info");
            if (!response.IsSuccessStatusCode)
                return new AuthenticationState(_anonymous);

            var user = await response.Content.ReadFromJsonAsync<UserInfoDto>();
            return CreateAuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous);
        }
    }

    // Hàm phụ trợ để tạo AuthState cho gọn
    private AuthenticationState CreateAuthenticationState(UserInfoDto? user)
    {
        if (user == null) return new AuthenticationState(_anonymous);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email ?? "")
        };

        var identity = new ClaimsIdentity(claims, "BffAuth");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserLoggedIn()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void NotifyUserLoggedOut()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}