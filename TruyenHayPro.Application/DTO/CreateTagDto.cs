using System.ComponentModel.DataAnnotations;

namespace TruyenHayPro.Application.DTO;

public class CreateTagDto
{
    [Required] public string Name { get; set; }
    public string? Description { get; set; }
}