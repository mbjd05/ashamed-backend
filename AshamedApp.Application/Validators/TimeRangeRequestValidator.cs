namespace AshamedApp.Application.Validators;

using DTOs;
using FluentValidation;

public class TimeRangeRequestValidator : AbstractValidator<TimeRangeRequest>
{
    public TimeRangeRequestValidator()
    {
        RuleFor(x => x.Topic).NotEmpty().WithMessage("The topic must not be empty.");
        RuleFor(x => x.Start).NotEmpty().WithMessage("The start date must not be empty.");
        RuleFor(x => x.End).NotEmpty().WithMessage("The end date must not be empty.");
        RuleFor(x => x.Start).LessThan(x => x.End).WithMessage("The start date must be earlier than the end date.");
        RuleFor(x => x)
            .Must(x => (x.End - x.Start).TotalDays <= 365)
            .WithMessage("The specified date range is too large. Please limit the range to a maximum of 1 year.");
        RuleFor(x => x.Start)
            .Must(date => date <= DateTime.Now)
            .WithMessage("The start date cannot be in the future.");

        RuleFor(x => x.End)
            .Must(date => date <= DateTime.Now)
            .WithMessage("The end date cannot be in the future.");
    }
}