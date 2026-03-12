using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Update
{
    public class UpdateRoutingFlowLinkedLevelCommandValidator : AbstractValidator<UpdateRoutingFlowLinkedLevelCommand>
    {
        public UpdateRoutingFlowLinkedLevelCommandValidator()
        {
            RuleFor(x => x.invInfoRoutingLevelID)
             .NotEmpty()
             .WithMessage("invInfoRoutingLevelID is required");
        }
    }
}
