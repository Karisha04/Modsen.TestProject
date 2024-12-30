using FluentValidation;
using Modsen.TestProject.Domain.Models;

namespace Modsen.TestProject.Application.Validators
{
    public class NewEventValidator : AbstractValidator<NewEvent>
    {
        public NewEventValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, NewEvent.MAX_NAME_LENGTH).WithMessage($"Name must be between 1 and {NewEvent.MAX_NAME_LENGTH} characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.DateAndTime)
                .GreaterThan(DateTime.Now).WithMessage("Event date and time must be in the future");

            RuleFor(x => x.Place)
                .NotEmpty().WithMessage("Place is required");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required");

            RuleFor(x => x.MaxParticipant)
                .GreaterThan(0).WithMessage("Max participants must be greater than 0");

            RuleFor(x => x.ImagePath)
                .Must(imagePath => string.IsNullOrEmpty(imagePath) || Uri.IsWellFormedUriString(imagePath, UriKind.Absolute))
                .WithMessage("Image path must be a valid URL");
        }
    }
}