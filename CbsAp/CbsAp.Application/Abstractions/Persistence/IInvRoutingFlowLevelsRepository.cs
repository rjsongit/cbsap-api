using CbsAp.Domain.Entities.Invoicing;

namespace CbsAp.Application.Abstractions.Persistence
{
    public interface IInvRoutingFlowLevelsRepository
    {
        Task<List<InvRoutingFlowLevels>> GetInvRoutingFlowLevelsById(long InvRoutingFlowID, CancellationToken cancellationToken);
    }
}
