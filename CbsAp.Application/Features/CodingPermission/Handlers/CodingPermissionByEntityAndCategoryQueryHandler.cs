using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;


namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionByEntityAndCategoryQueryHandler
        : IQueryHandler<CodingPermissionByEntityAndCategoryQuery, ResponseResult<IEnumerable<CodingPermissionDTO>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDimensionRepository _dimensionRepository;

        public CodingPermissionByEntityAndCategoryQueryHandler(IAccountRepository accountRepository, IDimensionRepository dimensionRepository)
        {
            _accountRepository = accountRepository;
            _dimensionRepository = dimensionRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionDTO>>> Handle(CodingPermissionByEntityAndCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = new List<CodingPermissionDTO>();

            switch (request.CategoryName.ToLower())
            {
                case "account":
                    var accounts = await _accountRepository.GetAccountsByEntityProfileIDAsync(request.EntityProfileID, cancellationToken);
                    result = accounts.Select(static i => new CodingPermissionDTO
                    {
                        EntityProfileID = i.EntityProfileID,
                        NameCode = $"{i.AccountID}-{i.AccountName}",
                        Name = i.AccountName,
                        Code = i.AccountID.ToString(),
                        OriginallyAssigned = true, // Set this based on your logic
                        Checked = true // Set this based on your logic
                    }).ToList();

                    return accounts.Any()
                        ? ResponseResult<IEnumerable<CodingPermissionDTO>>.SuccessRetrieveRecords(result, "Coding Permissions found")
                        : ResponseResult<IEnumerable<CodingPermissionDTO>>.NotFound("Coding Permissions not found");

                default:
                    var dimensions = await _dimensionRepository.GetDimensionByEntityProfileIDAsync(request.EntityProfileID, cancellationToken);
                    result = dimensions.Select(i => new CodingPermissionDTO
                    {
                        EntityProfileID = i.EntityProfileID,
                        NameCode = $"{i.DimensionCode}-{i.Name}",
                        Name = i.Name,
                        Code = i.DimensionCode,
                        OriginallyAssigned = true, // Set this based on your logic
                        Checked = true // Set this based on your logic
                    }).ToList();

                    return dimensions.Any()
                        ? ResponseResult<IEnumerable<CodingPermissionDTO>>.SuccessRetrieveRecords(result, "Coding Permissions found")
                        : ResponseResult<IEnumerable<CodingPermissionDTO>>.NotFound("Coding Permissions not found");
            }
        }
    }

}
