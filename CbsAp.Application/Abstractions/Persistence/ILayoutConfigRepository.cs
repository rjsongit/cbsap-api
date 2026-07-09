using CbsAp.Application.DTOs.UserManagement;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.Dashboard;
using CbsAp.Domain.Entities.LayoutConfigs;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface ILayoutConfigRepository
    {
        Task<LayoutConfig?> GetExistingUserConfig(string username);

    }
}