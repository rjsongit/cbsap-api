using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.Invoicing;
using CbsAp.Domain.Entities.LayoutConfigs;
using CbsAp.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CbsAp.Application.Features.Users.Commands.DeleteUser
{
    public class DeactivateUserCommandHandler
        : ICommandHandler<DeactivateUserCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitWork;
        private readonly ILayoutConfigRepository _layoutConfigRepository;
        private readonly ILogger<DeactivateUserCommandHandler> _logger;

        public DeactivateUserCommandHandler(
            IUnitofWork unitofWork,
            ILayoutConfigRepository layoutConfigRepository,
            ILogger<DeactivateUserCommandHandler> logger)
        {
            _layoutConfigRepository = layoutConfigRepository;
            _unitWork = unitofWork;
            _logger = logger;
        }

        public async Task<ResponseResult<string>> Handle(
            DeactivateUserCommand request,
            CancellationToken cancellationToken)
        {
            var userAccount = await _unitWork.GetRepository<UserAccount>()
                                             .GetByIdAsync(request.UserAccountID);
            if (userAccount == null)
            {
                _logger.LogWarning("User Account is not existed {@UserAccountID}",
                    request.UserAccountID);
                return ResponseResult<string>.NotFound("User Account is not existed");
            }

            var isUserHasData = await _unitWork.GetRepository<Invoice>().Query().AsNoTracking()
                                               .AnyAsync(i => i.CreatedBy == userAccount.UserID
                                               || i.LastUpdatedBy == userAccount.UserID || i.ApprovedUser == userAccount.UserAccountID, 
                                               cancellationToken);

            if (isUserHasData)
            {
                userAccount.IsUserPartialDeleted = true;
                userAccount.IsActive = false;
                userAccount.SetAuditFieldsOnUpdate(request.updatedBy);

                await _unitWork.GetRepository<UserAccount>().UpdateAsync(userAccount.UserAccountID, userAccount);
            }
            else
            {
                var layoutconfig = await _layoutConfigRepository.GetExistingUserConfig(userAccount.UserID);
                if(layoutconfig != null)
                {
                    await _unitWork.GetRepository<CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig>().DeleteAsync(layoutconfig);

                }
        
                await _unitWork.GetRepository<UserAccount>().DeleteAsync(userAccount);
            }

            if (await _unitWork.SaveChanges(string.Empty, string.Empty, cancellationToken))
            {
                return ResponseResult<string>.OK("User is successfully deleted");
            }

            _logger.LogError("Error on deleting UserAccount {@UserAccountID}", request.UserAccountID);
            return ResponseResult<string>.BadRequest("Error on deleting useraccount");
        }
    }
}