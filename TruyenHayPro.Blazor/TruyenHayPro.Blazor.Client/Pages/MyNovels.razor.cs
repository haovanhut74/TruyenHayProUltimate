using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class MyNovels : ComponentBase
{
    private List<NovelDto> novels = new();
    private bool isLoading = true;

    private string keyword = string.Empty;
    private string status = string.Empty;
    private string sort = "new";

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private async Task LoadData()
    {
        isLoading = true;
        novels = await NovelService.GetMyNovelsAsync();
        ApplyFilter();
        isLoading = false;
    }

    private void ApplyFilter()
    {
        var query = novels.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(n => n.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(n => n.StatusName == status);

        query = sort switch
        {
            "old" => query.OrderBy(n => n.CreatedDate),
            "views" => query.OrderByDescending(n => n.Views),
            _ => query.OrderByDescending(n => n.CreatedDate)
        };

        novels = query.ToList();
    }

    private async Task ConfirmDelete(Guid id)
    {
        var ok = await JS.InvokeAsync<bool>(
            "confirm",
            "Anh chắc chắn muốn xoá truyện này không?"
        );

        if (!ok) return;

        await NovelService.DeleteNovelAsync(id);

        novels.RemoveAll(x => x.Id == id);
        StateHasChanged();
    }
}