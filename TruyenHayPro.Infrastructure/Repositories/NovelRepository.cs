using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Infrastructure.Persistence;

namespace TruyenHayPro.Infrastructure.Repositories;

public class NovelRepository : INovelRepository
{
    private readonly ApplicationDbContext _context;

    public NovelRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Novel>> GetNovelsHomeAsync(int count)
    {
        return await _context.Novels
            .AsNoTracking()
            .Include(n => n.Category)
            .OrderByDescending(n => n.CreatedDate)
            .Take(count)
            .ToListAsync();
    }

    public IQueryable<Novel> Query()
    {
        return _context.Novels.AsNoTracking();
    }

    // 2. Lấy chi tiết truyện
    public async Task<Novel?> GetNovelByIdAsync(Guid id)
    {
        return await _context.Novels
            .AsNoTracking()
            .Include(n => n.Category) // Nạp Thể Loại
            .Include(n => n.Chapters) // Nạp luôn Danh sách chương
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<Guid> AddAsync(Novel novel)
    {
        _context.Novels.Add(novel);
        await _context.SaveChangesAsync();
        return novel.Id;
    }

    public async Task UpdateAsync(Novel novel)
    {
        _context.Novels.Update(novel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Novel novel)
    {
        _context.Novels.Remove(novel);
        await _context.SaveChangesAsync();
    }
    
}