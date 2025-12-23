using System.ComponentModel.DataAnnotations;

namespace TruyenHayPro.Application.DTO;

public class UpdateChapterDto
{
    [Required] public Guid Id { get; set; } // Id chương cần sửa
    public Guid NovelId { get; set; }
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nội dung không được để trống")]
    [MinLength(100, ErrorMessage = "Nội dung quá ngắn")]
    public string Content { get; set; } = string.Empty;

    public int ChapterNumber { get; set; }
}