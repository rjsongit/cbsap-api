using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlowLevels.Queries
{
    public class GetInvRoutingFlowLevelsByIDQueryHandler :
         IQueryHandler<GetInvRoutingFlowLevelsByIDQuery, ResponseResult<List<InvRoutingFlowLevelsDto>>>
    {
        private readonly IInvRoutingFlowLevelsRepository _invRoutingFlowLevelsRepository;

        public GetInvRoutingFlowLevelsByIDQueryHandler(IInvRoutingFlowLevelsRepository invRoutingFlowLevelsRepository)
        {
            _invRoutingFlowLevelsRepository = invRoutingFlowLevelsRepository;
        }

        public async Task<ResponseResult<List<InvRoutingFlowLevelsDto>>> Handle(GetInvRoutingFlowLevelsByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _invRoutingFlowLevelsRepository.GetInvRoutingFlowLevelsById(request.InvRoutingFlowID, cancellationToken);
            var dto = result.Select(dto => new InvRoutingFlowLevelsDto
            {
                InvRoutingFlowLevelID = dto.InvRoutingFlowLevelID,
                InvRoutingFlowID = dto.InvRoutingFlowID,
                RoleID = dto.RoleID,
                Level = dto.Level,
            }).ToList();

            return result.Count == 0 ?
                ResponseResult<List<InvRoutingFlowLevelsDto>>.NotFound("No Invoice Routing Flow") :
                ResponseResult<List<InvRoutingFlowLevelsDto>>.SuccessRetrieveRecords(dto);
        }
    }
}
