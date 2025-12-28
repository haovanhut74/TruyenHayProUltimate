using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TruyenHayPro.Application.Common.Admin.Interfaces.Services;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Domain.Enums;

namespace TruyenHayPro.Infrastructure.Admin.Services;

public class AdminNovelService : IAdminNovelService
{
    private readonly INovelRepository _novelRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public AdminNovelService(
        INovelRepository novelRepository,
        ITagRepository tagRepository,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _novelRepository = novelRepository;
        _tagRepository = tagRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<NovelDto>> GetAllNovelsAsync()
    {
        // Admin lấy hết, sắp xếp theo ngày tạo mới nhất
        var novels = await _novelRepository.Query()
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
        return _mapper.Map<List<NovelDto>>(novels);
    }

    public async Task<NovelDto?> GetNovelByIdAsync(Guid id)
    {
        var novel = await _novelRepository.GetNovelByIdAsync(id);
        return _mapper.Map<NovelDto>(novel);
    }

    public async Task<Guid> CreateNovelAsync(CreateNovelDto dto)
    {
        // Logic tạo mới (Copy từ NovelService cũ sang)
        var novel = new Novel
        {
            Title = dto.Title,
            Description = dto.Description,
            Author = dto.Author,
            CoverImage = dto.CoverImageUrl,
            Status = NovelStatus.OnGoing,
            Views = 0,
            Likes = 0,
            Rating = 0,
            CategoryId = dto.CategoryId,
            CreatedBy = _currentUser.UserId
        };

        if (dto.TagIds.Count > 0)
        {
            var tags = await _tagRepository.GetListByIdsAsync(dto.TagIds);
            foreach (var tag in tags) novel.Tags.Add(tag);
        }

        return await _novelRepository.AddAsync(novel);
    }

    public async Task UpdateNovelAsync(UpdateNovelDto dto)
    {
        var novel = await _novelRepository.GetNovelByIdAsync(dto.Id);
        if (novel == null) throw new Exception("Không tìm thấy truyện");

        // Admin có quyền sửa tất cả, không cần check CreatedBy
        novel.Title = dto.Title;
        novel.Description = dto.Description;
        novel.CategoryId = dto.CategoryId;
        novel.CoverImage = dto.CoverImageUrl;
        novel.LastModifiedAt = DateTimeOffset.UtcNow;

        await _novelRepository.UpdateAsync(novel);
    }

    public async Task DeleteNovelAsync(Guid id)
    {
        var novel = await _novelRepository.GetNovelByIdAsync(id);
        if (novel == null) throw new Exception("Không tìm thấy truyện");

        await _novelRepository.DeleteAsync(novel);
    }
}