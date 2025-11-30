using System;
using System.Collections.Generic;
using TruyenHayPro.Domain.Common.Models;
using TruyenHayPro.Domain.Enums;

namespace TruyenHayPro.Domain.Common.Entities;

public class Novel : BaseAuditableEntity
{
    public string Title { get; set; } = "Tên truyện";
    public string? Description { get; set; }
    public string Author { get; set; } = "Ẩn danh";
    public string? CoverImage { get; set; }

    public NovelStatus Status { get; set; } = NovelStatus.OnGoing;
    public long Views { get; set; } = 0;
    public long Likes { get; set; } = 0;
    public double Rating { get; set; } = 0;

    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}