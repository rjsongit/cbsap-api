using CbsAp.Application.Abstractions.Messaging;
using CbsAp.Application.Abstractions.Persistence;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.LayoutConfigs;
using CbsAp.Application.Features.Entity.Queries.GetEntityByRoleID;
using CbsAp.Application.Shared.ResultPatten;
using Mapster;



namespace CbsAp.Application.Features.LayoutConfig.Queries.GetLayoutConfigByUser
{
    public class GetLayoutConfigByUserQueryHandler
    : IQueryHandler<GetLayoutConfigByUserQuery,
    ResponseResult<LayoutConfigDTO>>
    {
        private readonly ILayoutConfigRepository _layoutConfigRepository;



        public GetLayoutConfigByUserQueryHandler(
        ILayoutConfigRepository layoutConfigRepository
        )
        {
            _layoutConfigRepository = layoutConfigRepository;
        }



        public async Task<ResponseResult<LayoutConfigDTO>> Handle(
        GetLayoutConfigByUserQuery request,
        CancellationToken cancellationToken
        )
        {

            var response = await _layoutConfigRepository.GetExistingUserConfig(request.userid);
            var result = new LayoutConfigDTO();

            if(response != null)
            {
                result.LayoutValue = response.LayoutValue;
            }

            return ResponseResult<LayoutConfigDTO>
            .OK(result);
        }
    }
}