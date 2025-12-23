using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class ReadChapter : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    private ChapterDetailDto? chapter;

    // --- STATE GIAO DIỆN ---
    private bool showSettings = false; // Trạng thái ẩn/hiện popup
    private string currentMode = "mode-dark";
    private string currentFont = "'Segoe UI', sans-serif"; // Mặc định
    private int fontSize = 20;
    private double currentLineHeight = 1.8;

    private bool _shouldScrollToTop;

    protected override async Task OnParametersSetAsync()
    {
        chapter = null;
        chapter = await ChapterService.GetChapterDetailAsync(Id);
        _shouldScrollToTop = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_shouldScrollToTop)
        {
            _shouldScrollToTop = false;
            await JS.InvokeVoidAsync("window.scrollTo", 0, 0);
        }
    }

    private string FormatContent(string content)
    {
        if (string.IsNullOrEmpty(content)) return "";
        return content.Replace("\n", "<br/><br/>");
    }

    private static string GetLink(Guid? id) => id.HasValue ? $"/chapter/{id}" : "#";

    // --- CÁC HÀM CÀI ĐẶT ---
    private void ToggleSettings() => showSettings = !showSettings;

    private void SetMode(string mode) => currentMode = mode;

    private void IncreaseFont()
    {
        if (fontSize < 36) fontSize += 2;
    }

    private void DecreaseFont()
    {
        if (fontSize > 14) fontSize -= 2;
    }

    private void IncreaseLineHeight()
    {
        if (currentLineHeight < 2.5) currentLineHeight = Math.Round(currentLineHeight + 0.1, 1);
    }

    private void DecreaseLineHeight()
    {
        if (currentLineHeight > 1.2) currentLineHeight = Math.Round(currentLineHeight - 0.1, 1);
    }
}