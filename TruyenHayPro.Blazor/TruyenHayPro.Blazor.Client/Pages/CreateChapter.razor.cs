using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class CreateChapter : ComponentBase
{
    [Parameter] public Guid NovelId { get; set; }
    private CreateChapterDto Model = new();
    private bool _isLoading = false;

    protected override void OnInitialized()
    {
        Model.NovelId = NovelId;
        // Gợi ý số chương tiếp theo: Có thể gọi API lấy MaxChapter + 1 (Bài tập về nhà nhé ^^)
        Model.ChapterNumber = 1;
    }

    private async Task HandleSubmit()
    {
        _isLoading = true;
        var result = await ChapterService.CreateChapterAsync(Model);
        _isLoading = false;

        if (result.IsSuccess)
        {
            // Thành công -> Quay về trang chi tiết truyện
            Navigation.NavigateTo($"/novel-detail/{NovelId}");
        }
        else
        {
            // Xử lý lỗi (Hiện Toast hoặc Alert)
            Console.WriteLine(string.Join(",", result.Errors));
        }
    }
}