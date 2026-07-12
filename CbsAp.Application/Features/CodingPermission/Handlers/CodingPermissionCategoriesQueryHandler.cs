using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionCategoriesQueryHandler
        : IQueryHandler<CodingPermissionCategoriesQuery, ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>>
    {
        private readonly IDimensionSetupRepository _dimensionSetupRepository;

        public CodingPermissionCategoriesQueryHandler(IDimensionSetupRepository dimensionSetupRepository)
        {
            _dimensionSetupRepository = dimensionSetupRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>> Handle(CodingPermissionCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dimensionsSetup = await _dimensionSetupRepository.GetDimensionByActiveAsync(cancellationToken);
                var result = dimensionsSetup.Select(i => new CodingPermissionCategoryDTO
                {
                    CategoryID = i.DimensionSetupId,
                    CategoryCode = i.DimensionSetupName,
                    CategoryName = i.DimensionSetupName,
                    EntityProfileID = 0
                }).ToList();

                // add "account" manually
                result.Add(new CodingPermissionCategoryDTO
                {
                    CategoryID = 0,
                    CategoryCode = "None",
                    CategoryName = "Account",
                    EntityProfileID = 0
                });

                return dimensionsSetup.Any()
                    ? ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>.SuccessRetrieveRecords(result.OrderBy(i => i.CategoryID), "Coding Categories found")
                    : ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>.OK("No data available");
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>.InternalServerError($"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
