using FluentValidation;

namespace CbsAp.Application.Features.Invoicing.InvRoutingFlows.Commands.UpdateInvoiceRoutingFlowID
{
    public class UpdateInvoiceRoutingFlowIDCommandValidator : AbstractValidator<UpdateInvoiceRoutingFlowIDCommand>
    {
        public UpdateInvoiceRoutingFlowIDCommandValidator()
        {
            RuleFor(x => x.invoiceID)
              .NotEmpty()
              .GreaterThan(0)
              .WithMessage("Invoice ID is required");

            RuleFor(x => x.invRoutingFlowID)
             .NotEmpty()
             .WithMessage("InvRoutingFlowID is required");
        }
    }
}
