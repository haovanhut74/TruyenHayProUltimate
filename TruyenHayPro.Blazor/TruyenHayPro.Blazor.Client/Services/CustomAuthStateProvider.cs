// using System.Net.Http.Json;
// using System.Security.Claims;
// using Microsoft.AspNetCore.Components.Authorization;
// using TruyenHayPro.Shared.DTO;
//
// namespace TruyenHayPro.Blazor.Client.Services;
//
// public class CustomAuthStateProvider : AuthenticationStateProvider
// {
//     private readonly HttpClient _http;
//     private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
//
//     public CustomAuthStateProvider(HttpClient http)
//     {
//         _http = http;
//     }
//
//     public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//     {
//         try
//         {
//             var response = await _http.GetAsync("/bff/auth/user-info");
//
//             if (!response.IsSuccessStatusCode)
//                 return new AuthenticationState(_anonymous);
//
//             var user = await response.Content.ReadFromJsonAsync<UserInfoDto>();
//             if (user == null)
//                 return new AuthenticationState(_anonymous);
//
//             var claims = new List<Claim>
//             {
//                 // 🔥 BẮT BUỘC – QUAN TRỌNG NHẤT
//                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//
//                 // Hiển thị
//                 new Claim(ClaimTypes.Name, user.FullName),
//                 new Claim(ClaimTypes.Email, user.Email ?? "")
//             };
//
//             var identity = new ClaimsIdentity(claims, "BffAuth");
//             var principal = new ClaimsPrincipal(identity);
//
//             return new AuthenticationState(principal);
//         }
//         catch
//         {
//             return new AuthenticationState(_anonymous);
//         }
//     }
//
//
//     public void NotifyUserLoggedIn()
//     {
//         NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//     }
//
//     public void NotifyUserLoggedOut()
//     {
//         NotifyAuthenticationStateChanged(
//             Task.FromResult(new AuthenticationState(_anonymous))
//         );
//     }
// }