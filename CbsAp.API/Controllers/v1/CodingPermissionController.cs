using Asp.Versioning;
using CbsAp.Application.Features.CodingPermission.Queries;
using CbsAp.Application.Features.PermissionManagement.Queries.GetAllOperations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CbsAp.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CodingPermissionController : BaseAPIController
    {
        private readonly IMediator _mediator;

        public CodingPermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("categories/{entityProfileID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryByEntityID(long entityProfileID)
        {
            var query = new CodingPermissionCategoryByEntityIDQuery(entityProfileID);
            var result = await _mediator.Send(query);

            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("entity/{entityProfileID}/category/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCodingPermissionsByEntityAndCategory(long entityProfileID, string categoryName)
        {
            var query = new CodingPermissionByEntityAndCategoryQuery(entityProfileID, categoryName);
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }

        [Authorize]
        [HttpGet("entities/role/{roleID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCodingEntitiesByRole(long roleID)
        {
            var query = new CodingPermissionEntitiesByRoleQuery(roleID);
            var result = await _mediator.Send(query);
            return CreateResponse(result);
        }
    }
}