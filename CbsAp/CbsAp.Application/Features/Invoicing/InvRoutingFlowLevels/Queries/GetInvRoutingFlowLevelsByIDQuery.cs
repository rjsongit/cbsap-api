using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlowLevels.Queries
{
    public record GetInvRoutingFlowLevelsByIDQuery(long InvRoutingFlowID)
        : IQuery<ResponseResult<List<InvRoutingFlowLevelsDto>>>
    {
    }
}
