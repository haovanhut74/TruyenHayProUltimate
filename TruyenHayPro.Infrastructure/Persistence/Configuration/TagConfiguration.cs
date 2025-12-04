using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Persistence.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name).HasMaxLength(50).IsRequired();

        // Không cần cấu hình quan hệ Many-to-Many phức tạp,
        // EF Core 10 tự động hiểu khi ta khai báo ICollection ở cả 2 bên.
    }
}