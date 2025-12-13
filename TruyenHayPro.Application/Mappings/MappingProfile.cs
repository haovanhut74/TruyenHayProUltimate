using AutoMapper;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1. Map Novel -> NovelDto
        CreateMap<Novel, NovelDto>()
            // Map tên thể loại, nếu null thì để chuỗi rỗng để tránh lỗi NullReference
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

        // 2. Map Chapter -> ChapterDto (QUAN TRỌNG: ĐÂY LÀ DÒNG BỊ THIẾU)
        // Nếu thiếu dòng này, AutoMapper sẽ crash khi gặp list Chapters trong NovelDto
        CreateMap<Chapter, ChapterDto>();

        // 3. Map Category (Cho các API khác nếu cần)
        CreateMap<Category, CategoryDto>();
    }
}