using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //Khai báo các DbSet ở đây
    public DbSet<Novel> Novels { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // --- BÍ KÍP QUAN TRỌNG ---
        // Dòng này sẽ tự động tìm tất cả các file Configuration (NovelConfig, ChapterConfig...)
        // trong Assembly hiện tại và áp dụng chúng.
        // Đạo hữu không cần add thủ công từng cái.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}