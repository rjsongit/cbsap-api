using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class InvInfoRoutingLevelRepository : IInvInfoRoutingLevelRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public InvInfoRoutingLevelRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<bool> InsertInvInfoRoutingLevelWithInvoiceAsync(long InvoiceID, long InvRoutingFlowLevelID)
        {
            var rowAffected = 0;

            try
            {
                rowAffected = await _dbcontext.Database.ExecuteSqlInterpolatedAsync
                ($"EXECUTE dbo.spInvInfoRoutingLevelWithInvoiceInsert @InvoiceID={InvoiceID}, @InvRoutingFlowLevelID={InvRoutingFlowLevelID}");
            }
            catch (Exception ex)
            {
                return false;
            }
        
            return rowAffected > 0;
        }

        public async Task<bool> UpdateInvInfoRoutingLevelStatusAsync(long invRoutingFlowLevelID, int invFlowStatus)
        {
            var rowAffected = 0;

            try
            {
                rowAffected = await _dbcontext.Database.ExecuteSqlInterpolatedAsync
                ($"EXECUTE dbo.spInvInfoRoutingLevelStatusUpdate @InvRoutingFlowLevelID={invRoutingFlowLevelID}, @InvFlowStatus={invFlowStatus}");
            }
            catch (Exception ex)
            {
                return false;
            }

            return rowAffected > 0;
        }
    }
}
