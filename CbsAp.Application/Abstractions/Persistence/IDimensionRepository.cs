using CbsAp.Domain.Entities.Dimensions;
using CbsAp.Application.DTOs.CodingPermission;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IDimensionRepository
    {
        Task<IEnumerable<Dimension>> GetDimensions(long entityProfileId, string? dimension, string? name, bool? isActive);

        IQueryable<Dimension> GetDimensionsAsQueryable();

        Task<IEnumerable<Dimension>> GetDimensionByEntityProfileIDAsync(long entityProfileId, CancellationToken token);
        Task<IEnumerable<Dimension>> GetDimensionByEntityAndNameCodeAsync(CodingPermissionFilterDTO filter, CancellationToken token);
    }
}
