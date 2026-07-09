
using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Features.CodingPermission.Command;
using CbsAp.Application.Shared.ResultPatten;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class SaveCodingPermissionCommandHandler : ICommandHandler<SaveCodingPermissionCommand, ResponseResult<string>>
    {
        private readonly ICodingPermissionRepository _codingPermissionRepository;
        private readonly ILogger<SaveCodingPermissionCommandHandler> _logger;

        public SaveCodingPermissionCommandHandler(ICodingPermissionRepository codingPermissionRepository, ILogger<SaveCodingPermissionCommandHandler> logger)
        {
            _logger = logger;
            _codingPermissionRepository = codingPermissionRepository;
        }

        public async Task<ResponseResult<string>> Handle(SaveCodingPermissionCommand request, CancellationToken cancellationToken)
        {
            var dtos = request.CodingPermissionDTOs;

            try
            {
                foreach (var dto in dtos)
                {
                    var entity = dto.Adapt<CbsAp.Domain.Entities.CodingPermissions.CodingPermissionAssigned>();
                    entity.IsAssigned = dto.Checked;
                    await _codingPermissionRepository.AddOrUpdateAsync(entity, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving coding permissions.");
                return ResponseResult<string>.BadRequest("Error saving coding permissions.", "SaveCodingPermissionCommandHandler");
            }

            return ResponseResult<string>.Success("Coding permissions saved successfully.");
        }
    }
}