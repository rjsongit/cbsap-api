using CbsAp.Domain.Entities.CodingPermissions;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface ICodingPermissionRepository
    {
        Task AddAsync(CodingPermissionAssigned entity, CancellationToken cancellationToken);

        Task AddOrUpdateAsync(CodingPermissionAssigned entity, CancellationToken cancellationToken);

        Task<IEnumerable<CodingPermissionAssigned>> GetAllAsync();

        Task<IEnumerable<CodingPermissionAssigned>> GetByEntityAndCategoryAsync(long entityProfileID, string categoryName);
    }
}
