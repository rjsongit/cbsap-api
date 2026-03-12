using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Create
{
    public class CreateRoutingFlowLinkedLevelCommandHandler : ICommandHandler<CreateRoutingFlowLinkedLevelCommand, ResponseResult<bool>>
    {
        private readonly IInvInfoRoutingLevelRepository _invInfoRoutingLevelRepository;

        public CreateRoutingFlowLinkedLevelCommandHandler(IInvInfoRoutingLevelRepository invInfoRoutingLevelRepositor)
        {
            _invInfoRoutingLevelRepository = invInfoRoutingLevelRepositor;
        }

        public async Task<ResponseResult<bool>> Handle(CreateRoutingFlowLinkedLevelCommand request, CancellationToken cancellationToken)
        {
            var rowAffected = await _invInfoRoutingLevelRepository.InsertInvInfoRoutingLevelWithInvoiceAsync(request.invoideID, request.invInfoRoutingLevelID);

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "invoice routing flow level"));
        }
    }
}
