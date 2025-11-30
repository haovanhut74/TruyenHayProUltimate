using System;

namespace TruyenHayPro.Domain.Common.Models;

//Thời gian trường hà
public abstract class BaseAuditableEntity
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
}