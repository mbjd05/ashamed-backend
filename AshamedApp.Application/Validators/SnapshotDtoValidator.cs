using AshamedApp.Application.DTOs;
using FluentValidation;

namespace AshamedApp.Application.Validators;

public class SnapshotDtoValidator : AbstractValidator<SnapshotDto>
{
    public SnapshotDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("The title must not be empty.");
        RuleFor(x => x.Description).NotNull().WithMessage("The description must not be null.");
        RuleFor(x => x.Title).MaximumLength(100).WithMessage("The title must not exceed 100 characters.");
        RuleFor(x => x.Description).MaximumLength(500).WithMessage("The description must not exceed 500 characters.");
    }
}