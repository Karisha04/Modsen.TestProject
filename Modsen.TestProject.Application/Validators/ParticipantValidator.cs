using FluentValidation;
using Modsen.TestProject.Application.Contracts;

public class ParticipantValidator : AbstractValidator<ParticipantRequest>
{
    public ParticipantValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 100).WithMessage("First name must be between 2 and 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now).WithMessage("Birth date must be in the past");

        RuleFor(x => x.RegistrationDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Registration date must be in the past or present");
    }
}