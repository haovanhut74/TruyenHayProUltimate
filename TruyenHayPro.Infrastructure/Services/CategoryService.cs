using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> CreateCategoryAsync(CreateCategoryDto dto)
    {
        // Map DTO sang Entity
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description
        };

        return await _categoryRepository.AddAsync(category);
    }
}