using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Persistence.Configuration;

public class NovelConfiguration : IEntityTypeConfiguration<Novel>
{
    public void Configure(EntityTypeBuilder<Novel> builder)
    {
        builder.ToTable("Novels"); //Tên bảng trong DB

        builder.HasKey(x => x.Id); // primary key

        // 3. Ràng buộc các cột (Đây chính là thay thế cho [Required])
        builder.Property(x => x.Title)
            .HasMaxLength(255) // Giới hạn độ dài
            .IsRequired();
        builder.Property(n => n.Description)
            .HasMaxLength(1000);

        // 4. Quan hệ: 1 Novel có nhiều Chapters
        // Khi xóa Novel -> Xóa luôn tất cả Chapter (Cascade Delete)
        builder.HasMany(x => x.Chapters)
            .WithOne(x => x.Novel)
            .HasForeignKey(x => x.NovelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Một Category có nhiều Novel
        builder.HasOne(n => n.Category)
            .WithMany(c => c.Novels)
            .HasForeignKey(n => n.CategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Xóa thể loại thì KHÔNG xóa truyện (để an toàn)
    }
}