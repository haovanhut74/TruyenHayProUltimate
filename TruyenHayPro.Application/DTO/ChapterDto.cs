namespace TruyenHayPro.Application.DTO;

public class ChapterDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public DateTime CreatedDate { get; set; }
}