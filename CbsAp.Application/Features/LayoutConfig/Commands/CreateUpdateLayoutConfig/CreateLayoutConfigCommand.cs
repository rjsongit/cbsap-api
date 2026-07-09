using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.LayoutConfigs;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.LayoutConfig.Commands.CreateUpdateLayoutConfig
{
    public record CreateLayoutConfigCommand(LayoutConfigDTO LayoutConfig, string CreatedBy) : ICommand<ResponseResult<bool>>
    {
    }
}
