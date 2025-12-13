using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Components.Shared;

public partial class NovelCard : ComponentBase
{
    [Parameter] public NovelDto Novel { get; set; } = null!;

    // 3. Hàm xử lý khi click
    private void GoToDetail()
    {
        // Chuyển hướng đến trang /novel/{Guid}
        Navigation.NavigateTo($"/novel/{Novel.Id}");
    }
}