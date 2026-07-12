using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.CodingPermissions;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface ICodingPermissionRepository
    {
        Task AddAsync(CodingPermissionAssigned entity, CancellationToken cancellationToken);

        Task AddOrUpdateAsync(CodingPermissionAssigned entity, string currentUser, CancellationToken cancellationToken);

        Task<IEnumerable<CodingPermissionAssigned>> GetAllAsync();

        Task<IEnumerable<CodingPermissionAssigned>> GetByEntityAndCategoryAsync(long entityProfileID, string categoryName, long roleID);

        Task<IEnumerable<CodingPermissionAssigned>> GetAllAssignedFilteredAsync(CodingPermissionFilterDTO filter);

        Task<PaginatedList<CodingPermissionDTO>> GetByEntityCategoryRolePagedAsync(CodingPermissionSearchDTO search, CancellationToken token);
    }
}
