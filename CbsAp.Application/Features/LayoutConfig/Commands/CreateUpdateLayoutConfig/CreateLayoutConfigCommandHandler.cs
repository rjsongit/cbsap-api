using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.Features.LayoutConfig.Commands.CreateUpdateLayoutConfig;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.LayoutConfig.Commands.CreateUpdateLayoutConfig
{
    public class CreateLayoutConfigCommandHandler : ICommandHandler<CreateLayoutConfigCommand, ResponseResult<bool>>
    {
        private readonly ILogger<CreateLayoutConfigCommandHandler> _logger;
        private readonly IUnitofWork _unitofWork;
        private readonly ILayoutConfigRepository _layoutConfigRepository;
        public CreateLayoutConfigCommandHandler(
           ILogger<CreateLayoutConfigCommandHandler> logger,
            IUnitofWork unitofWork,
            ILayoutConfigRepository layoutConfigRepository
            )
        {
           _logger = logger;
            _unitofWork = unitofWork;
            _layoutConfigRepository = layoutConfigRepository;

        }

        public async Task<ResponseResult<bool>> Handle(CreateLayoutConfigCommand request, CancellationToken cancellationToken)
        {

            var layoutConfig = await _layoutConfigRepository.GetExistingUserConfig(request.CreatedBy);

            var newconfig = new CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig();


            if (layoutConfig != null)
            {
                //update
                newconfig = new CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig
                {
                    LayoutConfigId = layoutConfig.LayoutConfigId,
                    Username = layoutConfig.Username,
                    LayoutValue = request.LayoutConfig.LayoutValue
                };
                newconfig.SetAuditFieldsOnUpdate(request.CreatedBy);
                await _unitofWork.GetRepository<CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig>().UpdateAsync(newconfig.LayoutConfigId, newconfig);
                await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
            }
            else
            {
                //Add
                newconfig = new CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig
                {
                    LayoutConfigId = 0,
                    Username = request.CreatedBy,
                    LayoutValue = request.LayoutConfig.LayoutValue
                };

                newconfig.SetAuditFieldsOnCreate(request.CreatedBy);
                await _unitofWork.GetRepository<CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig>().AddAsync(newconfig);
                await _unitofWork.SaveChanges(string.Empty, string.Empty, cancellationToken);

            }




       

            return ResponseResult<bool>.Created(MessageConstants.Message(MessageOperationType.Create, "LayoutConfig"));
        }
    }
}
