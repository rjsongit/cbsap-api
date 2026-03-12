namespace CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel
{
    public class InvInfoRoutingLevelDto
    {
        public long InvInfoRoutingLevelID { get; set; }

        public long? InvRoutingFlowID { get; set; }

        public long? InvoiceID { get; set; } 

        public long? SupplierInfoID { get; set; }  

        public long? KeywordID { get; set; }

        public long RoleID { get; set; }

        public int Level { get; set; }
    }
}
