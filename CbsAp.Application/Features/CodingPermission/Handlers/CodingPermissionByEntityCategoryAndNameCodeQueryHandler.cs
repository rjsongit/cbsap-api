using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.CodingPermission;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.CodingPermission.Handlers
{
    public class CodingPermissionByEntityCategoryAndNameCodeQueryHandler
        : IQueryHandler<CodingPermissionByEntityCategoryAndNameCodeQuery, ResponseResult<IEnumerable<CodingPermissionDTO>>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IDimensionRepository _dimensionRepository;
        private readonly ICodingPermissionRepository _codingPermissionRepository;

        public CodingPermissionByEntityCategoryAndNameCodeQueryHandler(IAccountRepository accountRepository
            , IDimensionRepository dimensionRepository
            , ICodingPermissionRepository codingPermissionRepository)
        {
            _accountRepository = accountRepository;
            _dimensionRepository = dimensionRepository;
            _codingPermissionRepository = codingPermissionRepository;
        }

        public async Task<ResponseResult<IEnumerable<CodingPermissionDTO>>> Handle(CodingPermissionByEntityCategoryAndNameCodeQuery request, CancellationToken cancellationToken)
        {
            var result = new List<CodingPermissionDTO>();
            var assignedPermissions = await _codingPermissionRepository.GetAllAsync();

            // build a lookup keyed by (EntityProfileID, Category, NameCode)
            var assignedLookup = assignedPermissions
                .ToDictionary(ap => (ap.EntityProfileID, ap.Category!.Replace(" ", string.Empty).ToLower(), ap.NameCode), ap => ap.IsAssigned);

            switch (request.filter.Category.ToLower())
            {
                case "account":
                    var accounts = await _accountRepository.GetAccountByEntityAndNameCodeAsync(request.filter, cancellationToken);
                    result = accounts.Select(i =>
                    {
                        var nameCode = $"{i.AccountID}-{i.AccountName}";
                        var key = (i.EntityProfileID, request.filter.Category.Replace(" ", string.Empty).ToLower(), nameCode);

                        if (assignedLookup.TryGetValue(key, out var isAssigned))
                        {
                            return new CodingPermissionDTO
                            {
                                ID = i.AccountID,
                                EntityProfileID = i.EntityProfileID,
                                Category = request.filter.Category,
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
                            Category = request.filter.Category,
                            NameCode = nameCode,
                            Name = i.AccountName,
                            Code = i.AccountID.ToString(),
                            OriginallyAssigned = false,
                            Checked = false
                        };
                    }).Where(x =>
                        (request.filter.IsAssigned && x.Checked) ||
                        (request.filter.IsUnassigned && !x.Checked)
                    ).ToList();

                    return result.Any()
                        ? ResponseResult<IEnumerable<CodingPermissionDTO>>.SuccessRetrieveRecords(result, "Coding Permissions found")
                        : ResponseResult<IEnumerable<CodingPermissionDTO>>.OK("Coding Permissions not found");

                default:
                    var dimensions = await _dimensionRepository.GetDimensionByEntityAndNameCodeAsync(request.filter, cancellationToken);
                    result = dimensions.Select(i =>
                    {
                        var nameCode = $"{i.DimensionCode}-{i.Name}";
                        var key = (i.EntityProfileID, request.filter.Category.Replace(" ", string.Empty).ToLower(), nameCode);

                        if (assignedLookup.TryGetValue(key, out var isAssigned))
                        {
                            return new CodingPermissionDTO
                            {
                                ID = i.DimensionID,
                                EntityProfileID = i.EntityProfileID,
                                Category = request.filter.Category,
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
                            Category = request.filter.Category,
                            NameCode = nameCode,
                            Name = i.Name,
                            Code = i.DimensionCode,
                            OriginallyAssigned = false,
                            Checked = false
                        };
                    }).Where(x =>
                        (request.filter.IsAssigned && x.Checked) ||
                        (request.filter.IsUnassigned && !x.Checked)
                    ).ToList();

                    return result.Any()
                        ? ResponseResult<IEnumerable<CodingPermissionDTO>>.SuccessRetrieveRecords(result, "Coding Permissions found")
                        : ResponseResult<IEnumerable<CodingPermissionDTO>>.OK("Coding Permissions not found");
            }
        }
    }
}
