using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Persistence.Configuration;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("Chapters"); //Tên bảng trong DB

        builder.HasKey(x => x.Id); // primary key

        // Ràng buộc các cột
        builder.Property(x => x.Title)
            .HasMaxLength(255) // Giới hạn độ dài
            .IsRequired();

        // Nội dung truyện
        builder.Property(x => x.Content)
            .HasMaxLength(10000)
            .IsRequired();

        // Đánh Index cho OrderIndex để sau này sort chương cho nhanh
        builder.HasIndex(c => new { c.OrderIndex, c.NovelId })
            .IsUnique();
    }
}