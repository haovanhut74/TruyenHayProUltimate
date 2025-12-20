using System.ComponentModel.DataAnnotations;

namespace TruyenHayPro.Application.DTO;

public class UpdateNovelDto
{
    [Required] public Guid Id { get; set; }

    [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public Guid CategoryId { get; set; }

    public string? CoverImageUrl { get; set; }
}