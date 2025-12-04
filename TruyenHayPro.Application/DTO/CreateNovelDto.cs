using System.ComponentModel.DataAnnotations;
using TruyenHayPro.Domain.Enums;

namespace TruyenHayPro.Application.DTO;

public class CreateNovelDto
{
    [Required]
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public string Author { get; set; }
    
    public string? CoverImageUrl { get; set; } // Tạm thời nhập link ảnh (string)
    
    public NovelStatus Status { get; set; } = NovelStatus.OnGoing;
    

    // --- QUAN TRỌNG ---
    [Required]
    public Guid CategoryId { get; set; } // Truyện thuộc thể loại nào?

    public List<Guid> TagIds { get; set; } = []; // Danh sách các Tag ID
}