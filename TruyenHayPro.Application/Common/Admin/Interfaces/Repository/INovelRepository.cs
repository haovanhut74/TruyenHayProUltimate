using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Common.Admin.Interfaces.Repository;

public interface INovelRepository
{
    Task<List<Novel>> GetAllAsync();
    

}