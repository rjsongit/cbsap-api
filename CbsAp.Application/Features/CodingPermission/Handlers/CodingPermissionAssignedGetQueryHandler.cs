using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionAssignedGetQueryHandler
        : IQueryHandler<CodingPermissionAssignedGetQuery, ResponseResult<IEnumerable<CodingPermissionDTO>>>
    {
        private readonly ICodingPermissionRepository _codingPermissionRepository;

        public CodingPermissionAssignedGetQueryHandler(ICodingPermissionRepository codingPermissionRepository)
        {
            _codingPermissionRepository = codingPermissionRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionDTO>>> Handle(CodingPermissionAssignedGetQuery request, CancellationToken cancellationToken)
        {
            var assigned = await _codingPermissionRepository.GetByEntityAndCategoryAsync(request.EntityProfileID, request.CategoryName, request.RoleID);
            var result = assigned.Select(i => new CodingPermissionDTO
            {
                ID = i.ID,
                EntityProfileID = i.EntityProfileID,
                Category = i.Category,
                NameCode = i.NameCode
            }).ToList();

            return ResponseResult<IEnumerable<CodingPermissionDTO>>.Success(result);
        }
    }
}
