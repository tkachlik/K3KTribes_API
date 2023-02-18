using Microsoft.AspNetCore.Mvc.Filters;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}