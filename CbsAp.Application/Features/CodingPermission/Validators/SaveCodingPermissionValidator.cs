using CbsAp.Application.Features.CodingPermission.Command;
using FluentValidation;

namespace CbsAp.Application.Features.CodingPermission.Validators
{
    public class SaveCodingPermissionValidator : AbstractValidator<SaveCodingPermissionCommand>
    {
        public SaveCodingPermissionValidator()
        {
            // target each element inside the CodingPermissions collection
            RuleForEach(p => p.CodingPermissionDTOs).ChildRules(permission =>
            {
                permission.RuleFor(x => x.ID)
                   .NotEmpty()
                   .WithMessage("ID is required");

                permission.RuleFor(x => x.NameCode)
                   .NotEmpty()
                   .WithMessage("Name Code is required");

                permission.RuleFor(x => x.EntityProfileID)
                   .NotEmpty()
                   .WithMessage("Entity is required");

                permission.RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Category is required");
            });
        }
    }
}
