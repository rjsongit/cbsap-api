using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Infrastracture.Contexts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class InvRoutingFlowLevelsRepository : IInvRoutingFlowLevelsRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public InvRoutingFlowLevelsRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<List<InvRoutingFlowLevels>> GetInvRoutingFlowLevelsById(long InvRoutingFlowID, CancellationToken cancellationToken)
        {
            var query = await _dbcontext.Levels
                .AsNoTracking()
                .AsQueryable()
                .AsExpandable()
                .Where(i => i.InvRoutingFlowID == InvRoutingFlowID).ToListAsync(cancellationToken);

            //var dto = query.Select(dto => new InvRoutingFlowLevels
            //{
            //    InvRoutingFlowLevelID = dto.InvRoutingFlowLevelID,
            //    InvRoutingFlowID = dto.InvRoutingFlowID,
            //    RoleID = dto.RoleID,
            //    Level = dto.Level,
            //});

            return query;
        }
    }
}
