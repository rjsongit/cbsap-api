using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.DTOs.TaxCodesManagement;
using CbsAp.Application.Shared.ResultPatten;

namespace CbsAp.Application.Features.TaxCodesManagement.Queries.Lookups
{
    public record GetTaxCodesLookupQuery(long entityId) 
        : IQuery<ResponseResult<IEnumerable< TaxCodeLookupDto>>>
    {
    }
}
