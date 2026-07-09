
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionCategoryByEntityIDQueryHandler
        : IQueryHandler<CodingPermissionCategoryByEntityIDQuery, ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>>
    {
        private readonly IDimensionRepository _dimensionRepository;

        public CodingPermissionCategoryByEntityIDQueryHandler(IDimensionRepository dimensionRepository)
        {
            _dimensionRepository = dimensionRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>> Handle(CodingPermissionCategoryByEntityIDQuery request, CancellationToken cancellationToken)
        {
            var dimensions = await _dimensionRepository.GetDimensionByEntityProfileIDAsync(request.EntityProfileID, cancellationToken);
            var result = dimensions.Select(i => new CodingPermissionCategoryDTO
            {
                CategoryID = i.DimensionID,
                CategoryCode = i.DimensionCode,
                CategoryName = i.Name,
                EntityProfileID = i.EntityProfileID
            }).ToList();

            return dimensions.Any()
                ? ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>.SuccessRetrieveRecords(result, "Coding Categories found")
                : ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>.NotFound("Coding Categories not found");
        }
    }
}