using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Update
{
    public class UpdateRoutingFlowLinkedLevelCommandHandler : ICommandHandler<UpdateRoutingFlowLinkedLevelCommand, ResponseResult<bool>>
    {
        private readonly IInvInfoRoutingLevelRepository _invInfoRoutingLevelRepository;

        public UpdateRoutingFlowLinkedLevelCommandHandler(IInvInfoRoutingLevelRepository invInfoRoutingLevelRepositor)
        {
            _invInfoRoutingLevelRepository = invInfoRoutingLevelRepositor;
        }

        public async Task<ResponseResult<bool>> Handle(UpdateRoutingFlowLinkedLevelCommand request, CancellationToken cancellationToken)
        {
            var rowAffected = await _invInfoRoutingLevelRepository.UpdateInvInfoRoutingLevelStatusAsync(request.invInfoRoutingLevelID, request.invFlowStatus);

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "invoice routing flow level"));
        }
    }
}
