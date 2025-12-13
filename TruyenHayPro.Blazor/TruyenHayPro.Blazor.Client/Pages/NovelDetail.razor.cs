using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class NovelDetail : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    private NovelDto? novel;
    private string activeTab = "intro";

    protected override async Task OnInitializedAsync()
    {
        // Gọi Service lấy thông tin truyện (đã kèm Chapters nhờ sửa DTO ở trên)
        novel = await NovelService.GetNovelByIdAsync(Id);
    }
}