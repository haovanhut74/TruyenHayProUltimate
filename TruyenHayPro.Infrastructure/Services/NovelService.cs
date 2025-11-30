using AutoMapper;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Infrastructure.Services;

public class NovelService : INovelService
{
    private readonly INovelRepository _novelRepository; // Thủ kho
    private readonly IMapper _mapper; // Máy biến hình

    public NovelService(INovelRepository novelRepository, IMapper mapper)
    {
        _novelRepository = novelRepository;
        _mapper = mapper;
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
}