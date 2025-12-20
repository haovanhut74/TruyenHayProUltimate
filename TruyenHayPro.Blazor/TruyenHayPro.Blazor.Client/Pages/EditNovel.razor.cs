using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class EditNovel : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    private UpdateNovelDto model = new();
    private List<CategoryDto> categories = [];
    private bool isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        var novel = await NovelService.GetNovelByIdAsync(Id);
        if (novel == null)
        {
            Navigation.NavigateTo("/not-found");
            return;
        }

        model = new UpdateNovelDto
        {
            Id = novel.Id,
            Title = novel.Title,
            Description = novel.Description,
            CategoryId = novel.CategoryId,
            CoverImageUrl = novel.CoverImage
        };

        categories = await NovelService.GetCategoriesAsync();
        isLoading = false;
    }

    private async Task HandleSubmit()
    {
        await NovelService.UpdateNovelAsync(model);
        Navigation.NavigateTo("/truyen-cua-toi");
    }

    private void GoBack()
    {
        Navigation.NavigateTo("/truyen-cua-toi");
    }
}