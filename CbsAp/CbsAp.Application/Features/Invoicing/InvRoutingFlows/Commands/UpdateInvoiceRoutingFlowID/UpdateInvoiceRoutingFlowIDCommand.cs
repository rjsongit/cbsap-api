using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.UpdateInvoiceRoutingFlowID
{
    public record UpdateInvoiceRoutingFlowIDCommand(long invoiceID, long invRoutingFlowID)
        : ICommand<ResponseResult<bool>>
    {
    }
}
