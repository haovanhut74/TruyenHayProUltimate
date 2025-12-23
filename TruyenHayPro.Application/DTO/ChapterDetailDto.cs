namespace TruyenHayPro.Application.DTO;

public class ChapterDetailDto
{
    public Guid Id { get; set; }
    public Guid NovelId { get; set; }
    public string NovelTitle { get; set; } = string.Empty; // Để hiển thị breadcrumb
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    // Navigation info
    public Guid? PreviousChapterId { get; set; }
    public Guid? NextChapterId { get; set; }
}