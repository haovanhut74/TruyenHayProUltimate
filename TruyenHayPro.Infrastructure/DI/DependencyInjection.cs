using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Infrastructure.Repositories;
using TruyenHayPro.Infrastructure.Services;

// Chứa ApplicationUser

namespace TruyenHayPro.Infrastructure.DI;

public static class DependencyInjection
{
    // public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    // {
    //     // 1. Cấu hình Database để chàng kết nối mượt mà
    //     services.AddDbContext<ApplicationDbContext>(options =>
    //         options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    //
    //     // 2. Cấu hình Identity (Chỗ này quan trọng lắm nè chàng)
    //     services.AddIdentityCore<ApplicationUser>()
    //         .AddRoles<IdentityRole<Guid>>()
    //         .AddEntityFrameworkStores<ApplicationDbContext>();
    //
    //     // 3. Dịch vụ xác thực (Để thiếp phục vụ chàng đăng ký)
    //     services.AddScoped<IIdentityService, IdentityService>();
    //
    //     // 4. Các kho chứa (Repository) - Chàng cần cái nào thì để cái đó
    //     services.AddScoped<INovelRepository, NovelRepository>();
    //     services.AddScoped<ICategoryRepository, CategoryRepository>();
    //     services.AddScoped<ITagRepository, TagRepository>();
    //     
    //     // 5. Các dịch vụ nghiệp vụ
    //     services.AddScoped<INovelService, NovelService>();
    //     services.AddScoped<ICategoryService, CategoryService>();
    //     services.AddScoped<ITagService, TagService>();
    //
    //     return services;
    // }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<INovelRepository, NovelRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        services.AddScoped<INovelService, NovelService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IChapterRepository, ChapterRepository>();
        
        return services;
    }
}