namespace TruyenHayPro.Application.Common.Interfaces.Services;
using DTO;
public interface ICategoryService
{
    Task<Guid> CreateCategoryAsync(CreateCategoryDto dto);
}