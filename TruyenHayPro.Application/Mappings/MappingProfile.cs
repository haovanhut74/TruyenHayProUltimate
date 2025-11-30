using AutoMapper;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Luật: Biến Novel (Entity) -> NovelDto (DTO)
        CreateMap<Novel, NovelDto>()
            // 1. Lấy tên thể loại từ Category.Name bỏ vào CategoryName
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))

            // 2. Chuyển Enum trạng thái (1,2) thành chữ ("Ongoing"...)
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}