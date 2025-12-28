using TruyenHayPro.Domain.Common.Entities;

namespace TruyenHayPro.Application.Common.Admin.Interfaces.Repository;

public interface IAdminNovelRepository
{
    Task<List<Novel>> GetAllAsync();
    

}