using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class ManageChapters : ComponentBase
{
    [Parameter] public Guid NovelId { get; set; }
    private NovelDto? novel;

    protected override async Task OnInitializedAsync()
    {
        novel = await NovelService.GetNovelByIdAsync(NovelId);
    }
}