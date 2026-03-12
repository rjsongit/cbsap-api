using CbsAp.Domain.Enums;

namespace CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel
{
    public class InvInfoRoutingLevelUpdateModel
    {
        public long InvRoutingFlowLevelID { get; set; }

        public InvFlowStatus InvFlowStatus { get; set; }
    }
}
