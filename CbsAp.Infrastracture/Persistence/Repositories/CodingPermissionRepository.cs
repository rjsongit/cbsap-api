using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.CodingPermissions;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class CodingPermissionRepository : ICodingPermissionRepository
    {
        private readonly IUnitofWork _unitofWork;

        public CodingPermissionRepository(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public async Task AddAsync(CodingPermissionAssigned entity, CancellationToken cancellationToken)
        {
            await _unitofWork.GetRepository<CodingPermissionAssigned>().AddAsync(entity);
            await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
        }

        public async Task AddOrUpdateAsync(CodingPermissionAssigned entity, CancellationToken cancellationToken)
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();

            var existing = await repo.FirstOrDefaultAsync(cp
                    => cp.EntityProfileID == entity.EntityProfileID
                    && cp.Category == entity.Category
                    && cp.NameCode == entity.NameCode);

            if (existing != null)
            {
                //existing.NameCode = entity.NameCode;
                //existing.Category = entity.Category;
                //existing.EntityProfileID = entity.EntityProfileID;

                existing.IsAssigned = entity.IsAssigned;
                existing.SetAuditFieldsOnUpdate("system");
                await repo.UpdateAsync(existing.ID, existing);
            }
            else
            {
                entity.ID = 0;
                entity.SetAuditFieldsOnCreate("system");
                await repo.AddAsync(entity);
            }

            await _unitofWork.SaveChanges("system", "coding permission", cancellationToken);
        }

        public async Task<IEnumerable<CodingPermissionAssigned>> GetAllAsync()
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();
            return await repo.GetAllAsync();
        }

        public async Task<IEnumerable<CodingPermissionAssigned>> GetByEntityAndCategoryAsync(long entityProfileID, string categoryName)
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();
            return await repo.FindAsync(i => i.EntityProfileID == entityProfileID && i.Category == categoryName && i.IsAssigned);
        }
    }
}