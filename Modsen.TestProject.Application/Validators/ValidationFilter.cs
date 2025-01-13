using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Modsen.TestProject.Application.Validators
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;
                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argument);

                    ValidationResult result = validator.Validate(validationContext);
                    if (!result.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(result.Errors);
                        return;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
