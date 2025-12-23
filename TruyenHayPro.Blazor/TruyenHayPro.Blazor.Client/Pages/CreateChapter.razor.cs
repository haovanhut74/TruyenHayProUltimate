using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class CreateChapter : ComponentBase
{
    [Parameter] public Guid NovelId { get; set; }
    private CreateChapterDto Model = new();
    private bool _isLoading = false;
    private string errorMessage = string.Empty;

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
        errorMessage = string.Empty;
        _isLoading = false;

        if (result.IsSuccess)
        {
            // Thành công -> Quay về trang chi tiết truyện
            Navigation.NavigateTo($"/manage-chapters");
        }
        else
        {
            // Xử lý lỗi (Hiện Toast hoặc Alert)
            errorMessage = string.Join(", ", result.Errors);
        }
    }
}