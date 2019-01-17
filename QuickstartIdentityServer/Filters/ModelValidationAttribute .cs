using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuickstartIdentityServer.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickstartIdentityServer.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (modelState != null && !modelState.IsValid)
            {
                string error = string.Empty;
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        error = state.Errors.First().Exception.Message;
                        context.Result = new JsonResult(new ErrorResult(error));
                        // throw new CustomException("9001", error);
                    }
                }
            }

            base.OnActionExecuting(context);
        }

    }
}
