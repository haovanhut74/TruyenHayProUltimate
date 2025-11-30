using System;
using TruyenHayPro.Domain.Common.Models;

namespace TruyenHayPro.Domain.Common.Entities;

public class Chapter : BaseAuditableEntity
{
    public string Title { get; set; } = "Tên chương";
    public string? Content { get; set; }
    public int WordCount { get; set; } = 0;

    //Thứ tự chương
    public int OrderIndex { get; set; }

    public Guid NovelId { get; set; }
    public Novel Novel { get; set; }
}