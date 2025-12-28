using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TruyenHayPro.Application.Common.Admin.Interfaces.Services;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Infrastructure.Admin.Services;
using TruyenHayPro.Infrastructure.Repositories;
using TruyenHayPro.Infrastructure.Services;

// Chứa ApplicationUser

namespace TruyenHayPro.Infrastructure.DI;

public static class DependencyInjection
{
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
        services.AddScoped<IAdminNovelService, AdminNovelService>();
        return services;
    }
}