namespace TruyenHayPro.Application.DTO;

public class CreateChapterDto
{
    public Guid NovelId { get; set; } // Chương này thuộc truyện nào
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset CreateAt { get; set; }
    public int ChapterNumber { get; set; } // Chương số mấy
}