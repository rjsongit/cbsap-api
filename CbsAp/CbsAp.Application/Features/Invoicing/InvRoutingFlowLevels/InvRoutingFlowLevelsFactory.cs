using CbsAp.Application.DTOs.Invoicing.InvRoutingFlow;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlowLevels
{
    public static class InvRoutingFlowLevelsFactory
    {
        public static InvRoutingFlowLevelsDto GetInvRoutingFlowLevelsDto(Domain.Entities.Invoicing.InvRoutingFlowLevels routingFlow)
        {
            ArgumentNullException.ThrowIfNull(routingFlow);
            return new InvRoutingFlowLevelsDto
            {
                InvRoutingFlowLevelID = routingFlow.InvRoutingFlowLevelID,
                InvRoutingFlowID = routingFlow.InvRoutingFlowID,
                RoleID = routingFlow.RoleID,
                Level = routingFlow.Level,
            };
        }
    }
}
