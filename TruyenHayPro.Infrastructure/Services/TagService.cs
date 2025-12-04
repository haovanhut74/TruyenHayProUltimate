using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Guid> CreateTagAsync(CreateTagDto dto)
    {
        var tag = new Tag
        {
            Name = dto.Name,
            Description = dto.Description
        };
        return await _tagRepository.AddAsync(tag);
    }
}