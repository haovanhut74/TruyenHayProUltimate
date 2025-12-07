using Microsoft.AspNetCore.Components;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Blazor.Client.Services;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class CreateNovel : ComponentBase
{
    // Code logic giữ nguyên như cũ
    private CreateNovelDto novelModel = new();
    private List<CategoryDto> categories = new();
    private string errorMessage = string.Empty;
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        if (NovelService is ClientNovelService clientService)
        {
            categories = await clientService.GetCategoriesAsync();
        }
    }

    private async Task HandleCreateNovel()
    {
        if (novelModel.CategoryId == Guid.Empty)
        {
            errorMessage = "Vui lòng chọn thể loại!";
            return;
        }

        isLoading = true;
        errorMessage = string.Empty;

        try
        {
            var id = await NovelService.CreateNovelAsync(novelModel);
            Navigation.NavigateTo("/");
        }
        catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        isLoading = false;
    }
}