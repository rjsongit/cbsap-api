using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;

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
            try
            {
                var entities = (await _entityRepository.GetCodingEntitiesByRoleAsync(request.RoleID));
                var result = entities.Select(i => new CodingPermissionEntityDTO
                {
                    EntityProfileID = i.EntityProfileID,
                    EntityName = i.EntityName,
                    EntityCode = i.EntityCode
                }).ToList();

                return entities.Any()
                    ? ResponseResult<IEnumerable<CodingPermissionEntityDTO>>.SuccessRetrieveRecords(result, "Coding Entities found")
                    : ResponseResult<IEnumerable<CodingPermissionEntityDTO>>.OK("No data available");
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<CodingPermissionEntityDTO>>.InternalServerError($"An error occurred while processing the request: {ex.Message}");
            }
        }
    }
}