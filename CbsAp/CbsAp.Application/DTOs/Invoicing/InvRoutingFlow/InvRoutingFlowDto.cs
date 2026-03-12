namespace CbsAp.Application.DTOs.Invoicing.InvRoutingFlow
{
    public class InvRoutingFlowDto
    {
        public long InvRoutingFlowID { get; set; }

        public string? InvRoutingFlowName { get; set; }

        public long? EntityProfileID { get; set; }

        public long? SupplierInfoID { get; set; }

        public bool IsActive { get; set; }

        public string? MatchReference { get; set; }

        public long RoleID { get; set; }

        public string? InvoiceID { get; set; }

        public string? KeywordID { get; set; }

        public string? InvFLowStatus { get; set; }

        public List<InvRoutingFlowLevelsDto>? InvRoutingFlowLevels { get; set; }
    }
}