using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using TruyenHayPro.Shared.DTO;

namespace TruyenHayPro.Blazor.Services;

public class PersistingAuthenticationStateProvider : IDisposable
{
    private readonly PersistentComponentState _state;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly PersistingComponentStateSubscription _subscription;

    public PersistingAuthenticationStateProvider(
        PersistentComponentState state,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _state = state;
        _authenticationStateProvider = authenticationStateProvider;
        // Đăng ký sự kiện: Khi app chuẩn bị render xong thì chạy hàm OnPersistingAsync
        _subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    private async Task OnPersistingAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            // Lấy thông tin user từ Claims của Server
            var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = user.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var fullName =
                user.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value; // Hoặc claim "FullName" tùy bạn lưu

            if (userId != null)
            {
                var userInfo = new UserInfoDto
                {
                    Id = Guid.Parse(userId),
                    Username = user.Identity.Name ?? "",
                    Email = email,
                    FullName = fullName ?? user.Identity.Name ?? ""
                };

                // Ghi dữ liệu vào State để Client đọc được
                _state.PersistAsJson("UserInfo", userInfo);
            }
        }
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}