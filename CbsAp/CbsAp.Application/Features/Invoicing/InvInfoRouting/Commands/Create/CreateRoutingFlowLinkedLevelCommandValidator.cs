using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvInfoRouting.Commands.Create
{
    public class CreateRoutingFlowLinkedLevelCommandValidator : AbstractValidator<CreateRoutingFlowLinkedLevelCommand>
    {
        public CreateRoutingFlowLinkedLevelCommandValidator()
        {
            RuleFor(x => x.invoideID)
               .NotEmpty()
               .WithMessage("Active is required");

            RuleFor(x => x.invInfoRoutingLevelID)
                .NotEmpty()
                .WithMessage("Routing Name is required");

            RuleFor(x => x.createdBy)
               .NotEmpty()
               .WithMessage("Match Reference is required");
        }
    }
}
