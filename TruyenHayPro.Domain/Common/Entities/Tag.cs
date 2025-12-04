using TruyenHayPro.Domain.Common.Models;

namespace TruyenHayPro.Domain.Common.Entities;

public class Tag : BaseAuditableEntity
{
    public string Name { get; set; } 
    public string? Description { get; set; }

    // Quan hệ Nhiều - Nhiều: Một Tag gắn cho nhiều Novel
    public ICollection<Novel> Novels { get; set; } = new List<Novel>();
}