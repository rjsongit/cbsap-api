using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Infrastracture.Contexts;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class CodingPermissionRepository : ICodingPermissionRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public CodingPermissionRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


    }
}
