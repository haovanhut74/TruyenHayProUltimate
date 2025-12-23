using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class EditChapter : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    private UpdateChapterDto model = new();
    private bool isLoading = true;
    private bool isSubmitting = false;
    private string errorMessage = "";
    private Guid novelId;

    protected override async Task OnInitializedAsync()
    {
        // Gọi service lấy chi tiết (Result<ChapterDetailDto>)
        var result = await ChapterService.GetChapterDetailAsync(Id);

        if (result.IsSuccess)
        {
            var chapter = result.Value; // Dùng .Value do Result<T>
            novelId = chapter.NovelId;
            model = new UpdateChapterDto
            {
                Id = chapter.Id,
                NovelId = chapter.NovelId,
                Title = chapter.Title,
                Content = chapter.Content,
                ChapterNumber = chapter.OrderIndex
            };
        }
        else
        {
            errorMessage = string.Join(", ", result.Errors);
        }

        isLoading = false;
    }

    private async Task HandleSubmit()
    {
        isSubmitting = true;
        var result = await ChapterService.UpdateChapterAsync(model);
        if (result.IsSuccess)
            Navigation.NavigateTo($"/novel/{novelId}");
        else
            errorMessage = string.Join(", ", result.Errors);
        isSubmitting = false;
    }

    private void GoBack() => Navigation.NavigateTo(novelId != Guid.Empty ? $"/novel/{novelId}" : "/");
}