namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IInvInfoRoutingLevelRepository
    {
        Task<bool> InsertInvInfoRoutingLevelWithInvoiceAsync(long invoiceID, long invRoutingFlowLevelID);

        Task<bool> UpdateInvInfoRoutingLevelStatusAsync(long invRoutingFlowLevelID, int invFlowStatus);
    }
}
