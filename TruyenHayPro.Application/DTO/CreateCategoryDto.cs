using System.ComponentModel.DataAnnotations;

namespace TruyenHayPro.Application.DTO;

public class CreateCategoryDto
{
    [Required] // Bắt buộc phải nhập
    public string Name { get; set; }
    
    public string? Description { get; set; }
}