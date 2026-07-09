
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Queries
{
    public record CodingPermissionByEntityCategoryAndNameCodeQuery(CodingPermissionFilterDTO filter)
        : IQuery<ResponseResult<IEnumerable<CodingPermissionDTO>>>
    { }
}