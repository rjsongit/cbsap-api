
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Queries
{
    public record CodingPermissionCategoryByEntityIDQuery(long EntityProfileID)
        : IQuery<ResponseResult<IEnumerable<CodingPermissionCategoryDTO>>>
    { }
}
