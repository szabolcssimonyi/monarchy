using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Monarchy.Gateway.Api.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;
        private readonly IWebHostEnvironment environment;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;
            this.environment = environment;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogError("Exception cought in global exception filter: {@Context}", context);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new BadRequestObjectResult(environment.IsDevelopment()
                ? context.Exception
                : (object)"Fatal error");
        }
    }
}
