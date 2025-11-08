namespace UltimateDotNetSkeleton.Presentation.ActionFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidationFilterAttribute : IActionFilter
	{
		public ValidationFilterAttribute()
		{
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var action = context.RouteData.Values["action"];
			var controller = context.RouteData.Values["controller"];

			var param = context.ActionArguments.SingleOrDefault(x => Convert.ToString(x.Value)!.Contains("Dto")).Value;

			if (param == null)
			{
				context.Result = new BadRequestObjectResult($"Object is null. Action: {action}, Controller: {controller}");
				return;
			}

			if (!context.ModelState.IsValid)
			{
				context.Result = new UnprocessableEntityObjectResult(context.ModelState);
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
		}
	}
}
