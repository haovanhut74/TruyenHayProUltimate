using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.DTO;
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

    // 2. Implement trong ChapterRepository
    public async Task<Chapter?> GetChapterDetailAsync(Guid id)
    {
        return await _context.Chapters
            .Include(c => c.Novel) // Include Novel để lấy tên truyện
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Chapter?> GetNextChapterAsync(Guid novelId, int currentOrderIndex)
    {
        return await _context.Chapters
            .AsNoTracking()
            .Where(c => c.NovelId == novelId && c.OrderIndex > currentOrderIndex)
            .OrderBy(c => c.OrderIndex) // Lấy thằng lớn hơn gần nhất
            .FirstOrDefaultAsync();
    }

    public async Task<Chapter?> GetPreviousChapterAsync(Guid novelId, int currentOrderIndex)
    {
        return await _context.Chapters
            .AsNoTracking()
            .Where(c => c.NovelId == novelId && c.OrderIndex < currentOrderIndex)
            .OrderByDescending(c => c.OrderIndex) // Lấy thằng nhỏ hơn gần nhất
            .FirstOrDefaultAsync();
    }

    // Implement chi tiết
    public async Task<Chapter?> GetChapterWithNovelAsync(Guid id)
    {
        return await _context.Chapters
            .Include(c => c.Novel) // Quan trọng: Include để check quyền sở hữu
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Chapter chapter)
    {
        _context.Chapters.Update(chapter);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Chapter chapter)
    {
        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync();
    }
    public async Task<List<ChapterDto>> GetListByNovelIdAsync(Guid novelId)
    {
        // Lấy danh sách chương của truyện, sắp xếp theo thứ tự (OrderIndex)
        var query = _context.Chapters
            .AsNoTracking()
            .Where(x => x.NovelId == novelId)
            .OrderBy(x => x.OrderIndex)
            .Select(x => new ChapterDto
            {
                Id = x.Id,
                Title = x.Title,
                OrderIndex = x.OrderIndex,
                CreatedDate = x.CreatedDate
            });

        return await query.ToListAsync();
    }
}