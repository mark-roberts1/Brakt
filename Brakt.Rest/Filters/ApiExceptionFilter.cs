using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brakt.Rest.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var error = new ApiError();

            if (context.Exception is ArgumentNullException argNullEx)
            {
                context.Exception = null;
                context.HttpContext.Response.StatusCode = 400;

                error.Message = $"{argNullEx.ParamName} cannot be null.";
            }
            else if (context.Exception is ArgumentException argEx)
            {
                context.Exception = null;
                context.HttpContext.Response.StatusCode = 409;

                error.Message = argEx.Message;
            }
            else
            {
                context.HttpContext.Response.StatusCode = 500;
                error.Message = "Internal Server Error";
            }

            context.Result = new JsonResult(error);

            await base.OnExceptionAsync(context);
        }
    }
}
