using AutoMapper;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Domain.Enums;

namespace TruyenHayPro.Infrastructure.Services;

public class NovelService : INovelService
{
    private readonly INovelRepository _novelRepository; // Thủ kho

    private readonly IMapper _mapper; // Máy biến hình

// --- THÊM CÁI NÀY ---
    private readonly ITagRepository _tagRepository;

    public NovelService(INovelRepository novelRepository, IMapper mapper, ITagRepository tagRepository)
    {
        _novelRepository = novelRepository;
        _mapper = mapper;
        _tagRepository = tagRepository;
    }

    // 1. Lấy danh sách cho trang chủ
    public async Task<List<NovelDto>> GetNovelsHomeAsync(int count)
    {
        // Bước 1: Sai thủ kho đi lấy hàng thô (Entity)
        var novels = await _novelRepository.GetNovelsHomeAsync(count);

        // Bước 2: Bỏ vào máy Mapper để biến thành DTO (Món ăn đẹp mắt)
        // Nó sẽ tự động biến List<Novel> -> List<NovelDto>
        return _mapper.Map<List<NovelDto>>(novels);
    }

    // 2. Lấy chi tiết truyện
    public async Task<NovelDto?> GetNovelByIdAsync(Guid id)
    {
        // Bước 1: Lấy hàng thô
        var novel = await _novelRepository.GetNovelByIdAsync(id);

        // Nếu không có truyện thì trả về null
        if (novel == null) return null;

        // Bước 2: Biến hình thành DTO
        return _mapper.Map<NovelDto>(novel);
    }

    public async Task<Guid> CreateNovelAsync(CreateNovelDto dto)
    {
        // 1. Tạo Entity từ DTO (Map thủ công cho chuẩn xác logic)
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
            CreatedDate = DateTimeOffset.UtcNow,
            LastModifiedAt = DateTimeOffset.UtcNow,
            CategoryId = dto.CategoryId // Gắn Category
        };

        // 2. Xử lý Tags (Nếu có chọn Tag)
        if (dto.TagIds.Count > 0)
        {
            // Gọi TagRepo lấy danh sách Tag thật từ DB
            var tags = await _tagRepository.GetListByIdsAsync(dto.TagIds);

            // Gắn vào truyện
            foreach (var tag in tags)
            {
                novel.Tags.Add(tag);
            }
        }

        // 3. Lưu vào DB
        return await _novelRepository.AddAsync(novel);
    }
}