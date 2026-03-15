using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.UpdateInvoiceRoutingFlowID
{
    public class UpdateInvoiceRoutingFlowIDCommandHandler : ICommandHandler<UpdateInvoiceRoutingFlowIDCommand, ResponseResult<bool>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public UpdateInvoiceRoutingFlowIDCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateInvoiceRoutingFlowIDCommand request, CancellationToken cancellationToken)
        {
            var rowAffected = await _invoiceRepository.UpdateInvoiceRoutingFlowIDAsync(request.invoiceID, request.invRoutingFlowID);

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "Invoice routing flow updated."));
        }
    }
}
