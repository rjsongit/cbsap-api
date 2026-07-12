using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Queries
{
    public record CodingPermissionSearchQuery : IQuery<ResponseResult<PaginatedList<CodingPermissionDTO>>>
    {
        public long EntityProfileID { get; set; }

        public long RoleID { get; set; }

        public string Category { get; set; } = string.Empty;

        #region Paging

        public int PageNumber { get; init; }

        public int PageSize { get; init; }

        public string? SortField { get; init; }

        public int? SortOrder { get; init; }

        #endregion
    }
}