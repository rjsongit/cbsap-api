using Asp.Versioning;
using CbsAp.Application.Configurations.constants;
using CbsAp.Application.DTOs.Entity;
using CbsAp.Application.DTOs.LayoutConfigs;
using CbsAp.Application.DTOs.RolesManagement;
using CbsAp.Application.Features.LayoutConfig.Commands.CreateUpdateLayoutConfig;
using CbsAp.Application.Features.LayoutConfig.Queries.GetLayoutConfigByUser;
using CbsAp.Application.Features.Roles.Command.CreateRole;
using CbsAp.Application.Features.Roles.Command.DeleteRole;
using CbsAp.Application.Features.Roles.Command.UpdateRole;
using CbsAp.Application.Features.Roles.Queries.Common;
using CbsAp.Application.Features.Roles.Queries.SearchActions;
using CbsAp.Application.Features.Roles.Queries.SearchHandler;
using CbsAp.Application.Features.Supplier.Commands.DeleteSupplier;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LayoutConfigController : BaseAPIController
    {
        private readonly ISender _mediator;

        public LayoutConfigController(ISender mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLayoutConfigByUser()
        {
            var query = new GetLayoutConfigByUserQuery(this.CurrentUser);
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLayoutConfig([FromBody] LayoutConfigDTO layoutConfigDTO)
        {
            var query = new CreateLayoutConfigCommand(layoutConfigDTO,this.CurrentUser);
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

    }
}