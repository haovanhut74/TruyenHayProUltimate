namespace TruyenHayPro.Application.DTO;

public class NovelDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public double Rating { get; set; }

    public List<ChapterDto> Chapters { get; set; } = [];
}