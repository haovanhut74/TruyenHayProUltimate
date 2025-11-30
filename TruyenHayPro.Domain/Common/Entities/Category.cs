using System.Collections.Generic;
using TruyenHayPro.Domain.Common.Models;

namespace TruyenHayPro.Domain.Common.Entities;

public class Category : BaseAuditableEntity
{
    public string Name { get; set; } = "Tên thể loại";
    public string? Description { get; set; }

    // Quan hệ: Một thể loại chứa danh sách các truyện
    public ICollection<Novel> Novels { get; set; } = new List<Novel>();
}