using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionSearchQueryHandler : IQueryHandler<CodingPermissionSearchQuery, ResponseResult<PaginatedList<CodingPermissionDTO>>>
    {
        private readonly ICodingPermissionRepository _codingPermissionRepository;

        public CodingPermissionSearchQueryHandler(ICodingPermissionRepository codingPermissionRepository)
        {
            _codingPermissionRepository = codingPermissionRepository;
        }

        public async Task<ResponseResult<PaginatedList<CodingPermissionDTO>>> Handle(CodingPermissionSearchQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _codingPermissionRepository.GetByEntityCategoryRolePagedAsync(new CodingPermissionSearchDTO
                    {
                        EntityProfileID = request.EntityProfileID,
                        RoleID = request.RoleID,
                        Category = request.Category,
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                        SortField = request.SortField,
                        SortOrder = request.SortOrder
                    }, cancellationToken);

                return result == null
                    ? ResponseResult<PaginatedList<CodingPermissionDTO>>.OK("No data available")
                    : ResponseResult<PaginatedList<CodingPermissionDTO>>.SuccessRetrieveRecords(result);
            }
            catch (Exception ex)
            {
                return ResponseResult<PaginatedList<CodingPermissionDTO>>.InternalServerError($"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}
