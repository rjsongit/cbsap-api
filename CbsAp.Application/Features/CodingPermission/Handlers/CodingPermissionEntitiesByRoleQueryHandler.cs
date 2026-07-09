
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Dimensions;

namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionEntitiesByRoleQueryHandler
        : IQueryHandler<CodingPermissionEntitiesByRoleQuery, ResponseResult<IEnumerable<CodingPermissionEntityDTO>>>
    {
        private readonly IEntityRepository _entityRepository;

        public CodingPermissionEntitiesByRoleQueryHandler(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionEntityDTO>>> Handle(CodingPermissionEntitiesByRoleQuery request, CancellationToken cancellationToken)
        {
            var entities = (await _entityRepository.GetEntitiesByRoleAsync(request.RoleID));
            var result = entities.Select(i => new CodingPermissionEntityDTO
            {
                EntityProfileID = i.EntityProfileID,
                EntityName = i.EntityName,
                EntityCode = i.EntityCode
            }).ToList();

            return entities.Any()
                ? ResponseResult<IEnumerable<CodingPermissionEntityDTO>>.SuccessRetrieveRecords(result, "Coding Entities found")
                : ResponseResult<IEnumerable<CodingPermissionEntityDTO>>.NotFound("Coding Entities not found");
        }
    }
}