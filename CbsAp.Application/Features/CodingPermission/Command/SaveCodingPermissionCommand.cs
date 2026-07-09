using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Command
{
    public record SaveCodingPermissionCommand(List<CodingPermissionDTO> CodingPermissionDTOs, string currentUser)
        : ICommand<ResponseResult<string>>
    {
    }
}
