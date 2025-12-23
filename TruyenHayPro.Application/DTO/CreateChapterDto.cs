using System.ComponentModel.DataAnnotations; // Nhớ thêm dòng này

namespace TruyenHayPro.Application.DTO;

public class CreateChapterDto
{
    public Guid NovelId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề chương")]
    [MaxLength(500, ErrorMessage = "Tiêu đề không được quá 200 ký tự")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nội dung chương không được để trống")]
    [MinLength(100, ErrorMessage = "Nội dung chương quá ngắn (tối thiểu 100 ký tự)")]
    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.UtcNow;
    
    [Range(1, int.MaxValue, ErrorMessage = "Số chương phải lớn hơn 0")]
    public int ChapterNumber { get; set; }
}