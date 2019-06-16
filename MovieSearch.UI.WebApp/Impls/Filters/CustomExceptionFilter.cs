using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using MovieSearch.Domain.Data.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApp.Impls.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            this.logger = logger;
        }
        
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            var ex = context.Exception;

            string message = string.Empty;
            string actionName = "Internal";

            var response = context.HttpContext.Response;

            if(ex is BusinessException)
            {
                message = ex.Message;
            }
            else if(ex is UnauthorizedAccessException)
            {
                message = "You are not authorized to take this action";
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                actionName = "Unauthorized";
            }
            else
            {
                message = "Unexpected error occured " + ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                actionName = "Internal";
            }

            context.ExceptionHandled = true;

            var exceptionDetails = new ExceptionDetails()
            {
                StatusCode = response.StatusCode,
                Message = message,
                StackTrace = ex.StackTrace
            };

            logger.LogError(ex, JsonConvert.SerializeObject(exceptionDetails));

            var routeValueDictionary = new RouteValueDictionary();

            routeValueDictionary.Add("controller", "Error");
            routeValueDictionary.Add("action", actionName);
            routeValueDictionary.Add("errorMessage", message);

            context.Result = new RedirectToRouteResult(routeValueDictionary);
        }
    }
}
