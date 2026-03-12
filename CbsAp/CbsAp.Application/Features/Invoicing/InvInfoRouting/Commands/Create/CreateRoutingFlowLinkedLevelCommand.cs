using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Create
{
    public record CreateRoutingFlowLinkedLevelCommand(long invoideID, long invInfoRoutingLevelID, string createdBy)
        : ICommand<ResponseResult<bool>>
    {
    }
}
