using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    //Khai báo các DbSet ở đây
    public DbSet<Novel> Novels { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // --- BÍ KÍP QUAN TRỌNG ---
        // Dòng này sẽ tự động tìm tất cả các file Configuration (NovelConfig, ChapterConfig...)
        // trong Assembly hiện tại và áp dụng chúng.
        // Đạo hữu không cần add thủ công từng cái.
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Đổi tên bảng (Lúc này các bảng UserRole, UserClaim... cũng tự động dùng Guid theo)
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles"); // Lưu ý: IdentityRole<Guid>
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
    }
}