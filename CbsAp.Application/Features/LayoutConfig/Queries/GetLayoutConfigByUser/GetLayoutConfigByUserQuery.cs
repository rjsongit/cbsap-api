using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.LayoutConfigs;
using CbsAp.Application.Shared.ResultPatten;



namespace CbsAp.Application.Features.LayoutConfig.Queries.GetLayoutConfigByUser
{
    public record GetLayoutConfigByUserQuery(string userid)
    : IQuery<ResponseResult<LayoutConfigDTO>>;
}