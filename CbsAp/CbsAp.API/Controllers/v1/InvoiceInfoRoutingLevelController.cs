using CbsAp.Application.DTOs.Invoicing.InvInfoRoutingLevel;
using CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Create;
using CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Update;
using CbsAp.Application.Features.Invoicing.InvRoutingFlowLevels.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CbsAp.API.Controllers.v1
{
    //[Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class InvoiceInfoRoutingLevelController : BaseAPIController
    {
        private readonly ISender _mediator;

        public InvoiceInfoRoutingLevelController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-routing")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateInvInfoRoutingLevel([FromBody] InvInfoRoutingLevelCreateModel model)
        {
            var serialize = JsonSerializer.Serialize(model);

            var param = new GetInvRoutingFlowLevelsByIDQuery(model.InvRoutingFlowID);
            var result = await _mediator.Send(param);

            foreach(var item in result.ResponseData)
            {
                var create = new CreateRoutingFlowLinkedLevelCommand(model.InvoiceID, item.InvRoutingFlowLevelID, this.CurrentUser);
                var response = await _mediator.Send(create);
            }

            return CreateResponse(result);
        }

        [HttpPut("update-routing-status")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateInvInfoRoutingLevelStatus([FromBody] InvInfoRoutingLevelUpdateModel model)
        {
            var serialize = JsonSerializer.Serialize(model);

            var param = new UpdateRoutingFlowLinkedLevelCommand(model.InvRoutingFlowLevelID, (int)model.InvFlowStatus, this.CurrentUser);
            var result = await _mediator.Send(param);

            return CreateResponse(result);
        }
    }
}
