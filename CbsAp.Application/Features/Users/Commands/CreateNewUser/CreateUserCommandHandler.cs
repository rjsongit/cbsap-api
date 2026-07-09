using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.NotificationStrategy;
using CbsAp.Application.Abstractions.NotificationStrategy.NotificationContext;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.Abstractions.Shared;
using CbsAp.Application.Configurations;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs;
using CbsAp.Application.DTOs.Shared;
using CbsAp.Application.Features.Users.Commands.Common;
using CbsAp.Application.Shared.Encryption;
using CbsAp.Application.Shared.Extensions;
using CbsAp.Application.Shared.ResultPatten;
using CbsAp.Domain.Entities.LayoutConfigs;
using CbsAp.Domain.Entities.RoleManagement;
using CbsAp.Domain.Entities.UserManagement;
using CbsAp.Domain.Enums;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;

namespace CbsAp.Application.Features.Users.Commands.CreateNewUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, ResponseResult<string>>
    {
        private readonly IUnitofWork _unitOfWork;

        private readonly ISender _mediator;

        private readonly IMapper _mapper;
        private readonly IHasher _hasher;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly INotificationContext _notificationContext;
        private readonly ILayoutConfigRepository _layoutConfigRepository;

        private readonly AppSettings _options;

        public CreateUserCommandHandler(
            IUnitofWork unitofWork,
            ISender mediator,
            IMapper mapper,
            IHasher hasher,
            IPasswordGenerator passwordGenerator,
            INotificationContext notificationContext,
            ILayoutConfigRepository layoutConfigRepository,
            IOptions<AppSettings> options
            )
        {
            _unitOfWork = unitofWork;
            _mediator = mediator;
            _mapper = mapper;
            _hasher = hasher;
            _passwordGenerator = passwordGenerator;
            _notificationContext = notificationContext;
            _options = options.Value;
            _layoutConfigRepository = layoutConfigRepository;
        }

        public async Task<ResponseResult<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            //validate new user if exists
            if (await IsUserExists(request))
            {
                return ResponseResult<string>.Confict("UserID or Email Address is already exist");
            }

            if (request.userDTO.UserRoles.Count == 0)
            {
                return ResponseResult<string>.BadRequest("User must contain at least 1 role");
            }

            var userAccount = request.userDTO.Adapt<UserAccount>();

            var passwordOptions = new PasswordOptions();
            var generatedPassword = _passwordGenerator.Generate(passwordOptions);
            var hashedPassword = _hasher.HashPasword(generatedPassword, out var salt);

            userAccount.UserLogInfo = new UserLogInfo()
            {
                UserID = request.userDTO.UserID,
                PasswordHash = hashedPassword,
                PasswordSalt = Convert.ToBase64String(salt),
                ConfirmationToken = Guid.NewGuid().ToString(),
                TokenGenerationTime = DateTimeOffset.UtcNow.AddDays(1),
                ActivateNewUser = true
            };

            userAccount.UserRoles = [.. request.userDTO.UserRoles.Select(roleID => new UserRole
                                        {
                                            RoleID = roleID
                                        })
                                    ];

            userAccount.SetAuditFieldsOnCreate(request.CreatedBy);

            await _unitOfWork.GetRepository<UserAccount>().AddAsync(userAccount);

            var isSuccess = await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);

            if (!isSuccess)
            {
                return ResponseResult<string>.BadRequest("Error adding new user account");
            }
            else
            {
                var layoutConfig = await _layoutConfigRepository.GetExistingUserConfig(request.userDTO.UserID);

                var newconfig = new CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig();
                
            
                if (layoutConfig != null)
                {
                    //update
                    newconfig = new CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig
                    {
                        LayoutConfigId = layoutConfig.LayoutConfigId,
                        Username = layoutConfig.Username,
                        LayoutValue = request.userDTO.LayoutConfigValue ?? 0
                    };
                    newconfig.SetAuditFieldsOnUpdate(request.CreatedBy);

                    await _unitOfWork.GetRepository<CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig>().UpdateAsync(newconfig.LayoutConfigId, newconfig);
                    await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
                }
                else
                {
                    //Add
                    newconfig = new CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig
                    {
                        LayoutConfigId = 0,
                        Username = request.userDTO.UserID,
                        LayoutValue = request.userDTO.LayoutConfigValue ?? 0
                    };

                    newconfig.SetAuditFieldsOnCreate(request.CreatedBy);

                    await _unitOfWork.GetRepository<CbsAp.Domain.Entities.LayoutConfigs.LayoutConfig>().AddAsync(newconfig);
                    await _unitOfWork.SaveChanges(string.Empty, string.Empty, cancellationToken);
                }


         

            }
            

            //TODO : Generate or log email that failed to send
            var isEmailSent = await SendNewUserNotificationAsync(userAccount, generatedPassword);

            return ResponseResult<string>.Created(MessageConstants.Message(MessageOperationType.Create, "user"));
        }

        private async Task<bool> SendNewUserNotificationAsync(UserAccount userAccount, string generatedPassword)
        {
            var actionStrategy = _notificationContext
                       .GetNotificationTypeStrategy(NotificationType.NewUserNotification);

            var emailBindData = new Dictionary<string, string> {
                        { "firstName" , userAccount.FirstName },
                        { "customerName", "Test Customer" }, // TODO : GET THE VALUE OF THE CUSTOMER set up
                        { "userID" , userAccount.UserID },
                        { "tempPassword", generatedPassword },
                        { "linkConfirmation",
                            $"{_options.WebUrl}auth/user-verification/{userAccount.UserLogInfo.ConfirmationToken}" +
                            $"/{userAccount.UserLogInfo.ActivateNewUser} "}
            };

            return await actionStrategy.SendNotificationAsync(userAccount.EmailAddress, emailBindData);
        }

        private async Task<bool> IsUserExists(CreateUserCommand request)
        {
            return await _unitOfWork.GetRepository<UserAccount>()
                .AnyAsync(u => u.UserID == request.userDTO.UserID || u.EmailAddress == request.userDTO.EmailAddress);
        }
    }
}
