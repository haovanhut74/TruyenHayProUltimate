using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Admin.Components.Pages;

public partial class Novels : ComponentBase
{
    private List<NovelDto>? _novels;
    
    
    protected override async Task OnInitializedAsync()
    {
        // Call Service instead of Repository
        // _novels = await AdminNovelRepository.GetAllAsync(); // OLD
        _novels = await AdminNovelService.GetAllNovelsAsync(); // NEW
    }

    private static async Task DeleteNovel(Guid id, string title)
    {
        // Tạm thời để hàm trống, mình sẽ làm chức năng Xóa có confirm ở bước sau
        Console.WriteLine($"Muốn xóa truyện: {title}");
    }
}