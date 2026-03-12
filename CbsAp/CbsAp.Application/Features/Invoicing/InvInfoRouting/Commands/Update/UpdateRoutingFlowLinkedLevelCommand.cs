using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Update
{
    public record UpdateRoutingFlowLinkedLevelCommand(long invInfoRoutingLevelID, int? invFlowStatus, string UpdateBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
