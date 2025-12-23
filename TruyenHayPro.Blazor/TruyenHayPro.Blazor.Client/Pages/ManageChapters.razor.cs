using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class ManageChapters
{
    [Parameter] public Guid NovelId { get; set; }

    private List<ChapterDto> chapters = new();
    private bool isLoading = true;
    private string novelTitle = "";
    private string searchText = "";

    // Sửa: Dùng OrderIndex và CreatedDate đúng theo ChapterDto.cs
    private IEnumerable<ChapterDto> FilteredChapters =>
        string.IsNullOrWhiteSpace(searchText)
            ? chapters.OrderBy(c => c.OrderIndex)
            : chapters.Where(c => c.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                  || c.OrderIndex.ToString().Contains(searchText))
                .OrderBy(c => c.OrderIndex);

    protected override async Task OnInitializedAsync()
    {
        try
        {
            isLoading = true;

            // Lấy thông tin Novel
            var novelResult = await NovelService.GetNovelByIdAsync(NovelId);
            // Sửa: Result dùng IsSuccess và Value
            if (novelResult != null && novelResult.Id != Guid.Empty)
            {
                novelTitle = novelResult.Title;
            }

            // Lấy danh sách Chapter
            var result = await ChapterService.GetChaptersByNovelIdAsync(NovelId);
            if (result.IsSuccess)
            {
                chapters = result.Value ?? new List<ChapterDto>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task DeleteChapter(Guid chapterId)
    {
        bool confirmed = await JS.InvokeAsync<bool>("confirm",
            "Bạn có chắc muốn xóa chương này không? Hành động này không thể hoàn tác.");

        if (confirmed)
        {
            var result = await ChapterService.DeleteChapterAsync(chapterId);

            // Sửa: Result dùng IsSuccess
            if (result.IsSuccess)
            {
                chapters.RemoveAll(x => x.Id == chapterId);
                StateHasChanged();
            }
            else
            {
                // Sửa: Result dùng mảng Errors[]
                var msg = result.Errors?.FirstOrDefault() ?? "Có lỗi xảy ra";
                await JS.InvokeVoidAsync("alert", $"Lỗi: {msg}");
            }
        }
    }

    private void CreateChapter() => Navigation.NavigateTo($"/create-chapter/{NovelId}");
    private void EditChapter(Guid id) => Navigation.NavigateTo($"/edit-chapter/{id}");
    private void GoBack() => Navigation.NavigateTo("/my-novels");
}