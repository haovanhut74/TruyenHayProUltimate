using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Infrastructure.Persistence;

namespace TruyenHayPro.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Tag tag)
    {
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag.Id;
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        return await _context.Tags.AsNoTracking().ToListAsync();
    }
    
    public async Task<List<Tag>> GetListByIdsAsync(List<Guid> ids)
    {
        // Lấy những tag nào mà Id nằm trong danh sách ids gửi vào
        return await _context.Tags
            .Where(t => ids.Contains(t.Id))
            .ToListAsync();
    }
}