using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MovieSearch.Domain.Data.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieSearch.UI.WebApi.Impls.Filters
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

            HttpStatusCode status = HttpStatusCode.InternalServerError;

            if(ex is BusinessException)
            {
                message = ex.Message;
                status = HttpStatusCode.InternalServerError;
            }
            else if (ex is UnauthorizedAccessException)
            {
                message = "You are not authorized to take this action";
                status = HttpStatusCode.Unauthorized;
            }
            else
            {
                message = "Unexpected error occured. " + ex.Message;
                status = HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;

            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            var exceptionDetails = new ExceptionDetails()
            {
                StatusCode = (int)status,
                Message = message,
                StackTrace = ex.StackTrace
            };

            var exMessage = JsonConvert.SerializeObject(exceptionDetails);

            logger.LogError(ex, exMessage);

            response.WriteAsync(exMessage);
        }
    }
}
