using CbsAp.Application.DTOs.DimensionSetup;
using CbsAp.Application.Shared;
using CbsAp.Domain.Entities.DimensionSetup;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IDimensionSetupRepository
    {
        Task<DimensionSetupDto?> GetDimensionSetupByID(long dimensionSetupId);
        Task<PaginatedList<DimensionSetupListDto>> SearchDimensionSetupWithPagination(
            string? dimensionSetupName,
            string? dimensionSetupValue,
            int pageNumber,
            int pageSize,
            string? sortField,
            int? sortOrder,
            CancellationToken token);

        Task<IEnumerable<DimensionSetup>> GetDimensionByActiveAsync(CancellationToken token);
    }
}