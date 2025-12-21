using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Infrastructure.Persistence;

namespace TruyenHayPro.Infrastructure.Repositories;

public class ChapterRepository : IChapterRepository
{
    private readonly ApplicationDbContext _context;

    public ChapterRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Chapter> AddAsync(Chapter chapter)
    {
        await _context.Chapters.AddAsync(chapter);
        await _context.SaveChangesAsync();
        return chapter;
    }

    public async Task<bool> IsChapterNumberExistsAsync(Guid novelId, int chapterNumber)
    {
        return await _context.Chapters
            .AnyAsync(c => c.NovelId == novelId && c.OrderIndex == chapterNumber);
    }
}