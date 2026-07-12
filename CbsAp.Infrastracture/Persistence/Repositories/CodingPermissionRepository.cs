using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Shared;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Domain.Entities.CodingPermissions;
using LinqKit;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddOrUpdateAsync(CodingPermissionAssigned entity, string currentUser, CancellationToken cancellationToken)
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();

            var existing = await repo.FirstOrDefaultAsync(cp
                    => cp.EntityProfileID == entity.EntityProfileID
                    && cp.Category == entity.Category
                    && cp.NameCode == entity.NameCode
                    && cp.RoleID == entity.RoleID);

            if (existing != null)
            {
                existing.IsAssigned = entity.IsAssigned;
                existing.SetAuditFieldsOnUpdate(currentUser);
                await repo.UpdateAsync(existing.ID, existing);
            }
            else
            {
                entity.ID = 0;
                entity.SetAuditFieldsOnCreate(currentUser);
                await repo.AddAsync(entity);
            }

            await _unitofWork.SaveChanges(currentUser, "coding permission", cancellationToken);
        }

        public async Task<IEnumerable<CodingPermissionAssigned>> GetAllAsync()
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();
            return await repo.GetAllAsync();
        }

        public async Task<IEnumerable<CodingPermissionAssigned>> GetByEntityAndCategoryAsync(long entityProfileID, string categoryName, long roleID)
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();
            return await repo.FindAsync(i => i.EntityProfileID == entityProfileID
                && i.Category!.Replace(" ", string.Empty).ToLower().Contains(categoryName.Replace(" ", string.Empty).ToLower())
                && i.RoleID == roleID
                && i.IsAssigned);
        }

        public async Task<IEnumerable<CodingPermissionAssigned>> GetAllAssignedFilteredAsync(CodingPermissionFilterDTO filter)
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();
            return await repo.FindAsync(i => i.EntityProfileID == filter.EntityProfileID
                && i.RoleID == filter.RoleID
                && i.Category!.Replace(" ", string.Empty).ToLower().Contains(filter.Category.Replace(" ", string.Empty).ToLower())
                && i.IsAssigned);
        }

        public async Task<PaginatedList<CodingPermissionDTO>> GetByEntityCategoryRolePagedAsync(CodingPermissionSearchDTO search, CancellationToken token)
        {
            var repo = _unitofWork.GetRepository<CodingPermissionAssigned>();

            ExpressionStarter<CodingPermissionAssigned> predicate = 
                PredicateBuilder.New<CodingPermissionAssigned>(i => i.EntityProfileID == search.EntityProfileID 
                    && i.Category!.Replace(" ", string.Empty).ToLower() == search.Category.Replace(" ", string.Empty).ToLower()
                    && i.RoleID == search.RoleID
                    && i.IsAssigned);

            var query = repo.FindQueryAsync(predicate);
           
            var sortDictionary = new Dictionary<string, string>() {
                { "entityProfileID", "entityProfileID" },
                { "category", "category" },
                { "nameCode", "nameCode" },
              };

            search.SortField = sortDictionary.ContainsKey(search.SortField ?? string.Empty)
                            ? sortDictionary[search.SortField ?? string.Empty]
                            : null;

            if (string.IsNullOrEmpty(search.SortField))
                query = query.OrderByDescending(p => p.LastUpdatedDate ?? p.CreatedDate);

            var pagedList = await query.Select(i => new CodingPermissionDTO
            {
                ID = i.ID,
                EntityProfileID = i.EntityProfileID,
                Category = i.Category,
                NameCode = i.NameCode
            })
            .ToListAsync();

            return await pagedList.OrderByDynamic(search.SortField, search.SortOrder).ToPaginatedListAsync(search.PageNumber, search.PageSize, token);
        }
    }
}