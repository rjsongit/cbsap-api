using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Update
{
    public record UpdateRoutingFlowLinkedLevelCommand(long invInfoRoutingLevelID, int? invFlowStatus, string UpdateBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
