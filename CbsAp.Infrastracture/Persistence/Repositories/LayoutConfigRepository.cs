using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Domain.Entities.Dashboard;
using CbsAp.Domain.Entities.LayoutConfigs;
using CbsAp.Infrastracture.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CbsAp.Infrastracture.Persistence.Repositories
{
    public class LayoutConfigRepository : ILayoutConfigRepository
    {
        private readonly ApplicationDbContext _dbcontext;

        public LayoutConfigRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<LayoutConfig?> GetExistingUserConfig(string username)
        {
            return await _dbcontext.LayoutConfigs.FirstOrDefaultAsync(x => x.Username == username) ?? null;
        }
    }
}