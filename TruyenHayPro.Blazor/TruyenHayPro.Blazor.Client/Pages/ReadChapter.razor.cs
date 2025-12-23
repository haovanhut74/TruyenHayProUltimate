using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Blazor.Client.Pages;

public partial class ReadChapter : ComponentBase
{
    [Parameter] public Guid Id { get; set; }

    private ChapterDetailDto? chapter;

    // --- STATE GIAO DIỆN ---
    private bool showSettings; // Trạng thái ẩn/hiện popup
    private string currentMode = "mode-dark";
    private string currentFont = "'Segoe UI', sans-serif"; // Mặc định
    private int fontSize = 20;
    private double currentLineHeight = 1.8;

    private bool _shouldScrollToTop;

    private string LineHeightCSS =>
        currentLineHeight.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

    private int WordCount => string.IsNullOrWhiteSpace(chapter?.Content)
        ? 0
        : chapter.Content.Split([' ', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries).Length;

    protected override async Task OnParametersSetAsync()
    {
        chapter = null;
        chapter = await ChapterService.GetChapterDetailAsync(Id);
        _shouldScrollToTop = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // 👇 1. LẦN ĐẦU VÀO TRANG: Tải cài đặt cũ lên
            await LoadSettings();
        }

        if (_shouldScrollToTop)
        {
            _shouldScrollToTop = false;
            await JS.InvokeVoidAsync("window.scrollTo", 0, 0);
        }
    }

    public class ReaderSettings
    {
        public string Mode { get; set; } = "mode-dark";
        public string Font { get; set; } = "'Segoe UI', sans-serif";
        public int Size { get; set; } = 20;
        public double LineHeight { get; set; } = 1.8;
    }

    private async Task SaveSettings()
    {
        var settings = new ReaderSettings
        {
            Mode = currentMode,
            Font = currentFont,
            Size = fontSize,
            LineHeight = currentLineHeight
        };

        // Serialize sang JSON và lưu vào LocalStorage
        var json = System.Text.Json.JsonSerializer.Serialize(settings);
        await JS.InvokeVoidAsync("localStorage.setItem", SETTING_KEY, json);
    }

    private const string SETTING_KEY = "user_reader_settings";

    private async Task LoadSettings()
    {
        try
        {
            var json = await JS.InvokeAsync<string>("localStorage.getItem", SETTING_KEY);
            if (!string.IsNullOrEmpty(json))
            {
                var settings = System.Text.Json.JsonSerializer.Deserialize<ReaderSettings>(json);
                if (settings != null)
                {
                    currentMode = settings.Mode;
                    currentFont = settings.Font;
                    fontSize = settings.Size;
                    currentLineHeight = settings.LineHeight;

                    // Báo cho giao diện cập nhật lại ngay lập tức
                    StateHasChanged();
                }
            }
        }
        catch
        {
            // Nếu lỗi (vd: JSON cũ hỏng) thì dùng mặc định, kệ nó
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

    private void SetMode(string mode)
    {
        currentMode = mode;
        _ = SaveSettings(); // 👇 Lưu ngay khi đổi
    }

    // Khi đổi Font trong dropdown
    private void OnFontChange(ChangeEventArgs e)
    {
        currentFont = e.Value?.ToString() ?? "'Segoe UI', sans-serif";
        _ = SaveSettings(); // 👇 Lưu ngay
    }

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