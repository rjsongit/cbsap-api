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
        private readonly ICodingPermissionRepository _codingPermissionRepository;

        public CodingPermissionByEntityAndCategoryQueryHandler(IAccountRepository accountRepository
            , IDimensionRepository dimensionRepository
            , ICodingPermissionRepository codingPermissionRepository)
        {
            _accountRepository = accountRepository;
            _dimensionRepository = dimensionRepository;
            _codingPermissionRepository = codingPermissionRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionDTO>>> Handle(CodingPermissionByEntityAndCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = new List<CodingPermissionDTO>();
            var assignedPermissions = await _codingPermissionRepository.GetAllAsync();

            // build a lookup keyed by (EntityProfileID, Category, NameCode)
            var assignedLookup = assignedPermissions
                .ToDictionary(ap => (ap.EntityProfileID, ap.Category, ap.NameCode), ap => ap.IsAssigned);

            switch (request.CategoryName.ToLower())
            {
                case "account":
                    var accounts = await _accountRepository.GetAccountsByEntityProfileIDAsync(request.EntityProfileID, cancellationToken);
                    result = accounts.Select(i =>
                    {
                        var nameCode = $"{i.AccountID}-{i.AccountName}";
                        var key = (i.EntityProfileID, request.CategoryName, nameCode);

                        if (assignedLookup.TryGetValue(key, out var isAssigned))
                        {
                            return new CodingPermissionDTO
                            {
                                ID = i.AccountID,
                                EntityProfileID = i.EntityProfileID,
                                Category = request.CategoryName,
                                NameCode = nameCode,
                                Name = i.AccountName,
                                Code = i.AccountID.ToString(),
                                OriginallyAssigned = isAssigned, // exists and matches IsAssigned in other table
                                Checked = isAssigned
                            };
                        }

                        return new CodingPermissionDTO
                        {
                            ID = i.AccountID,
                            EntityProfileID = i.EntityProfileID,
                            Category = request.CategoryName,
                            NameCode = nameCode,
                            Name = i.AccountName,
                            Code = i.AccountID.ToString(),
                            OriginallyAssigned = false,
                            Checked = false
                        };
                    }).ToList();

                    return accounts.Any()
                        ? ResponseResult<IEnumerable<CodingPermissionDTO>>.SuccessRetrieveRecords(result, "Coding Permissions found")
                        : ResponseResult<IEnumerable<CodingPermissionDTO>>.NotFound("Coding Permissions not found");

                default:
                    var dimensions = await _dimensionRepository.GetDimensionByEntityProfileIDAsync(request.EntityProfileID, cancellationToken);
                    result = dimensions.Select(i =>
                    {
                        var nameCode = $"{i.DimensionCode}-{i.Name}";
                        var key = (i.EntityProfileID, request.CategoryName, nameCode);

                        if (assignedLookup.TryGetValue(key, out var isAssigned))
                        {
                            return new CodingPermissionDTO
                            {
                                ID = i.DimensionID,
                                EntityProfileID = i.EntityProfileID,
                                Category = request.CategoryName,
                                NameCode = nameCode,
                                Name = i.Name,
                                Code = i.DimensionCode,
                                OriginallyAssigned = isAssigned, // exists and matches IsAssigned in other table
                                Checked = isAssigned
                            };
                        }

                        return new CodingPermissionDTO
                        {
                            ID = i.DimensionID,
                            EntityProfileID = i.EntityProfileID,
                            Category = request.CategoryName,
                            NameCode = nameCode,
                            Name = i.Name,
                            Code = i.DimensionCode,
                            OriginallyAssigned = false,
                            Checked = false
                        };
                    }).ToList();

                    return dimensions.Any()
                        ? ResponseResult<IEnumerable<CodingPermissionDTO>>.SuccessRetrieveRecords(result, "Coding Permissions found")
                        : ResponseResult<IEnumerable<CodingPermissionDTO>>.NotFound("Coding Permissions not found");
            }
        }
    }

}
